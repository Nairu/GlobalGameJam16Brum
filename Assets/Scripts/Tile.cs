using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;

public enum TileTypes
{
    [Description("Empty")]
    Empty = 0,
    [Description("Dirt")]
    Dirt = 1,
    [Description("Tunnel")]
    Tunnel = 2,
    [Description("Dorm")]
    Dorm = 3,
    [Description("SummonRoom")]
    SummonRoom = 4,
    [Description("TunnelStart")]
    TunnelStart = 5,
    [Description("DugDirt")]
    DugDirt = 6,
    [Description("MessHall")]
    MessHall = 7,
    [Description("Dungeon")]
    Dungeon = 8,
    [Description("CthuluShrine")]
    CthuluShrine = 9,
    [Description("RecreationRoom")]
    RecreationRoom = 10
}

public class Tile : MonoBehaviour {

    public string TileName;

    public Vector2 Pos;
    public string TileType;
    public bool isWalkable = false;    

    public bool canUp = false;
    public bool canDown = false;
    public bool canLeft = false;
    public bool canRight = false;

    public int goldCost;
    public int soulsCost;

    private int _roomToPlace;

    MapManager map;

    public bool AllowsVerticalMove = false;

    public List<BaseJob> myJobs;

	// Use this for initialization
	void Start () {
        map = GameObject.FindObjectOfType<MapManager>();

	}

    void OnMouseDown()
    {
        Debug.Log(TileName + " Clicked");

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _roomToPlace = Camera.main.GetComponent<CameraMoveController>().RoomToPlace;
            int _goldCost = Camera.main.GetComponent<CameraMoveController>().GoldCost;
            int _soulsCost = Camera.main.GetComponent<CameraMoveController>().SoulsCost;

            switch (TileType)
            {
                case "Tunnel":
                    Debug.Log("Tunnel" + " Clicked");
                        TunnelClicked();
                    break;
                case "TunnelStart":
                    Debug.Log("Tunnel" + " Clicked");
                        TunnelClicked();
                    break;
                case "Dirt":
                    Debug.Log("Dirt" + " Clicked");
                    DirtClicked();
                    break;
                case "DugDirt":
                    Debug.Log("Empty Clicked");
                        DugDirtClicked(_goldCost, _soulsCost);                   
                    break;
                case "Empty":
                    Debug.Log("Empty Clicked");
                        DugDirtClicked(_goldCost, _soulsCost);                    
                    break;
                default:
                    Debug.Log("Room Clicked");
                    RoomClicked();
                    break;
            }
        }
    }

    void TunnelClicked()
    {
        // Check for dirt below
        string tileToCheck = "Tile_" + Pos.x + "_" + (Pos.y - 2.0f);
        GameObject go = GameObject.Find(tileToCheck);
        Debug.Log("Y Depth: " + map.yDepth + " and absolute value " + (Mathf.Abs(map.yDepth / 2)));
        if (go != null)
        {
            Tile tile = go.GetComponent<Tile>();
            if (tile.TileType == "Dirt")
            {
                if (Camera.main.GetComponent<GameResourceManager>().SpendResources((goldCost * (Mathf.Abs(map.yDepth / 2))), soulsCost))
                {
                    Debug.Log("Change to Ladder");
                    map.ChangeTile(tile.Pos, "Tunnel");
                    map.yDepth -= 2;
                }
                else
                {
                    Debug.Log("Not enough gold!");
                }
            }
        }
    }

    void DirtClicked()
    {
        if (this.Pos.y < map.yDepth || this.Pos.y > -2)
        {
            Debug.Log("No Ladder currently on this level: " + this.Pos.y);
            return;
        }

        //start logic to add job to the queue to destroy this dirt, 
        // ONLY if the tile in question has a walkable path to a ladder
        if (IsLadderReachable() != -1)
        {
            // Must check if we have discovered a secret! Oooooohh!
            TileTypes t = checkForSecret(Pos);

            Debug.Log("change Dirt to " + t.ToString());
            map.ChangeTile(Pos, Enumerations.GetEnumDescription(t));
        }
    }

    void DugDirtClicked(int _goldCost, int _soulsCost)
    {
        if (this.Pos.y < map.yDepth || this.Pos.y > -2)
            return;

        Debug.Log("Change to " + Enumerations.GetEnumDescription((TileTypes)_roomToPlace));

        if (this.Pos.y < -2f)
        {
            if (((TileTypes)_roomToPlace) == TileTypes.TunnelStart
                && (map.GetTileAt(Mathf.FloorToInt(this.Pos.x), Mathf.FloorToInt(this.Pos.y + 2)).TileType ==
                                 Enumerations.GetEnumDescription(TileTypes.Tunnel)) ||
                    map.GetTileAt(Mathf.FloorToInt(this.Pos.x), Mathf.FloorToInt(this.Pos.y + 2)).TileType ==
                                 Enumerations.GetEnumDescription(TileTypes.TunnelStart))
            {
                _roomToPlace = (int)TileTypes.Tunnel;
            }
        }
        
        if (Camera.main.GetComponent<GameResourceManager>().SpendResources(_goldCost, _soulsCost))
        {
            map.ChangeTile(Pos, Enumerations.GetEnumDescription((TileTypes)_roomToPlace));

            UpdateRoomCounts((TileTypes)_roomToPlace, 1);
            // increment number of prisons if necessary 
           // if ((TileTypes)_roomToPlace == TileTypes.Dungeon)
           //     map.prisonCount++;
 
        }
        else
            Debug.Log("Not enough gold!");
    }

    void RoomClicked()
    {
        Camera.main.GetComponent<GUIManager>().OpenPanel(TileName, TileType);
    }

    TileTypes checkForSecret(Vector2 pos)
    {
        // Cthulu's Shrine
        // Can only appear when y < -10 
        if (pos.y < map.CthuluLimit && map.CthuluFound == false)
        {
            int rand = Random.Range(1, 10);
            Debug.Log("Random number: " + rand);
            int y = Mathf.Abs((int)pos.y);
            Debug.Log("Y Coord: " + y);

            int marker = y + rand - map.CthuluLimit;
            Debug.Log("marker: " + marker);

            if (marker >= (map.CthuluLimit * 2))
            {
                map.CthuluFound = true;
                return TileTypes.CthuluShrine;
            }

        }

        // No secret found, return empty dirt
        return TileTypes.DugDirt;
    }

    public void DestroyRoom()
    {
        // Refund half the initial gold cost
        Camera.main.GetComponent<GameResourceManager>().AddGold(this.goldCost / 2);
        map.ChangeTile(Pos, Enumerations.GetEnumDescription(TileTypes.Empty));
        
        UpdateRoomCounts(TileType.GetEnumFromDescription<TileTypes>(), -1);
        // decrement number of prisons if necessary 
        //if (map.GetTileAt((int)Pos.x, (int)Pos.y).TileType == TileTypes.Dungeon.ToString())
        //    map.prisonCount--;
    }

    void UpdateRoomCounts(TileTypes type, int modifier)
    {

        switch (type)
        {
            case TileTypes.SummonRoom:
                break;
            case TileTypes.Dorm:
                map.dormCount += modifier;
                Debug.Log("Dorm count is now: " + map.dormCount);
                break;
            case TileTypes.MessHall:
                map.messCount += modifier;
                Debug.Log("Mess Hall count is now: " + map.messCount);
                break;
            case TileTypes.Dungeon:
                map.prisonCount += modifier;
                Debug.Log("Prison count is now: " + map.prisonCount);
                break;
            case TileTypes.RecreationRoom:
                map.recreationCount += modifier;
                Debug.Log("Recreation count is now: " + map.recreationCount);
                break;
            case TileTypes.CthuluShrine:
                break;
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public int IsLadderReachable()
    {
        // Firsttry pathing towards the centre
        int dir = 3;
        if (Pos.x > 0)
            dir = -3;

        //Tile examine = this;
        Debug.Log("Pathing check: "  + this.TileName);
        if (ExamineReachable(this, dir))
            return dir;
        else
        {
            // repeat, pathing the oposite direction
            dir *= -1;
           // examine = this;
            if (ExamineReachable(this, dir))
                return dir;
        }

        // Can't find a ladder in either direction, the tile is not currently connected
        Debug.Log("No Path to " + this.TileName);
        return -1;
    }

    bool ExamineReachable(Tile examine, int dir)
    {
        int checkX = (int)Pos.x + dir;
        examine = map.GetTileAt(checkX, (int)Pos.y);
        while (examine.isWalkable)
        {            
            if (!examine.isWalkable)
            {
                return false;
            }

            if (examine.TileType == TileTypes.Tunnel.ToString())
                return true;

            checkX += dir;
            examine = map.GetTileAt(checkX, (int)Pos.y);
        }

        return false;
    }




    public void CheckOptions()
    {
        //Check UP
        // ensure not on top row
        if (Pos.y < -2)
        {
            if (TileType == Enumerations.GetEnumDescription(TileTypes.Tunnel))
                canUp = true;
            else
                canUp = false;
        }

        // Check Down
        //ensure there is a tile below
        Tile tile = map.GetTileAt((int)Pos.x, (int)Pos.y - 2);
        if (tile != null)
        {
            if ((TileType == Enumerations.GetEnumDescription(TileTypes.Tunnel) ||            //
                TileType == Enumerations.GetEnumDescription(TileTypes.TunnelStart)) &&    // If this tile has a ladder
                (tile.TileType == Enumerations.GetEnumDescription(TileTypes.Tunnel)))        // AND The tile below has a tall ladder 
            {
                canDown = true;
            }
            else
                canDown = false;
        }
        
        // Check Left
        // Ensure there is a Tile to the left
        tile = map.GetTileAt((int)Pos.x - 3, (int)Pos.y);
        if (tile != null)
        {
            if (tile.isWalkable)
                canLeft = true;
            else
                canLeft = false;
        }

        // Check Right
        // Ensure there is a Tile to the right
        tile = map.GetTileAt((int)Pos.x + 3, (int)Pos.y);
        if (tile != null)
        {
            if (tile.isWalkable)
                canLeft = true;
            else
                canLeft = false;
        }

    }
}
