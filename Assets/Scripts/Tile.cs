using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.ComponentModel;
using System.Reflection;

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
    DugDirt = 6
}

public class Tile : MonoBehaviour {

    public string TileName;

    public Vector2 Pos;
    public string TileType;
    public bool isWalkable = false;

    public int goldCost;
    public int soulsCost;

    private int _roomToPlace;

    MapManager map;

    public bool AllowsVerticalMove = false;

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
        if (IsReachable())
            map.ChangeTile(Pos, Enumerations.GetEnumDescription(TileTypes.Empty));
    }

    void DugDirtClicked(int _goldCost, int _soulsCost)
    {
        if (this.Pos.y < map.yDepth || this.Pos.y > -2)
            return;

        Debug.Log("Change to " + Enumerations.GetEnumDescription((TileTypes)_roomToPlace));
        if (Camera.main.GetComponent<GameResourceManager>().SpendResources(_goldCost, _soulsCost))
        {
            map.ChangeTile(Pos, Enumerations.GetEnumDescription((TileTypes)_roomToPlace));
        }
        else
            Debug.Log("Not enough gold!");
    }

    void RoomClicked()
    {
        if (Enumerations.GetEnumDescription((TileTypes)_roomToPlace) == TileTypes.Empty.ToString())
        {
            // Refund half the initial gold cost
            Camera.main.GetComponent<GameResourceManager>().AddGold( this.goldCost / 2);
            map.ChangeTile(Pos, Enumerations.GetEnumDescription((TileTypes)_roomToPlace));
        }
    }

    // Update is called once per frame
    void Update () {
	
        
	}

    bool IsReachable()
    {
        // Firsttry pathing towards the centre
        int dir = 3;
        if (Pos.x > 0)
            dir = -3;

        //Tile examine = this;
        Debug.Log("Pathing check: "  + this.TileName);
        if (ExamineReachable(this, dir))
            return true;
        else
        {
            // repeat, pathing the oposite direction
            dir *= -1;
           // examine = this;
            if (ExamineReachable(this, dir))
                return true;
        }

        // Can't find a ladder in either direction, the tile is not currently connected
        Debug.Log("No Path to " + this.TileName);
        return false;
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

}
