using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public string TileName;

    public Vector2 Pos;
    public string TileType;

    MapManager map;

	// Use this for initialization
	void Start () {
        map = GameObject.FindObjectOfType<MapManager>();

	}

    void OnMouseDown()
    {
        Debug.Log(TileName + " Clicked");

        switch (TileType)
        {
            case "Tunnel":
                Debug.Log("Tunnel" + " Clicked");
                TunnelClicked();
                break;
            case "Dirt":
                Debug.Log("Dirt" + " Clicked");
                DirtClicked();
                break;
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
                Debug.Log("Change to Ladder");
                map.ChangeTile(tile.Pos, "Tunnel");
            }
        }
    }

    void DirtClicked()
    {
         Debug.Log("Change to Dormitory");
         map.ChangeTile(Pos, "Dorm");
    }

    // Update is called once per frame
    void Update () {
	
        
	}


}
