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
    SummonRoom = 4
}

public class Tile : MonoBehaviour {

    public string TileName;

    public Vector2 Pos;
    public string TileType;

    public int goldCost;
    public int soulsCost;

    private int _roomToPlace;

    MapManager map;

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
                case "Dirt":
                    Debug.Log("Dirt" + " Clicked");
                    DirtClicked(_goldCost, _soulsCost);
                    break;
            }
        }
    }

    void TunnelClicked()
    {
        // Check for dirt below
        string tileToCheck = "Tile_" + Pos.x + "_" + (Pos.y - 2.0f);
        GameObject go = GameObject.Find(tileToCheck);
        if (go != null)
        {
            Tile tile = go.GetComponent<Tile>();
            if (tile.TileType == "Dirt")
            {
                if (Camera.main.GetComponent<GameResourceManager>().SpendResources(goldCost, soulsCost))
                {
                    Debug.Log("Change to Ladder");
                    map.ChangeTile(tile.Pos, "Tunnel");
                }
                else
                {
                    Debug.Log("Not enough gold!");
                }
            }
        }
    }

    void DirtClicked(int _goldCost, int _soulsCost)
    {
         Debug.Log("Change to " + Enumerations.GetEnumDescription((TileTypes)_roomToPlace));        
        if (Camera.main.GetComponent<GameResourceManager>().SpendResources(_goldCost, _soulsCost))
        {
            map.ChangeTile(Pos, Enumerations.GetEnumDescription((TileTypes)_roomToPlace));
        }
        else
            Debug.Log("Not enough gold!");
    }

    // Update is called once per frame
    void Update () {
	
        
	}


}
