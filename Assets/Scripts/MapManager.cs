using UnityEngine;
using System.Collections;
using System.Linq;

public class MapManager : MonoBehaviour {

    public int numTilesY;    
    public int numTilesX;

	// Use this for initialization
	void Start () {
        SpawnTiles();


	}

    public void SpawnTiles()
    {
        numTilesY = Mathf.CeilToInt(Camera.main.orthographicSize) * 2;
        numTilesX = Mathf.CeilToInt(Screen.width / Screen.height * numTilesY) * 3;

        int x = Mathf.FloorToInt(Camera.main.transform.position.x - numTilesX / 3);

        for (; x < Camera.main.transform.position.x + Mathf.CeilToInt(numTilesX / 2) + 1; x = x+3)
        {
            int y = Mathf.FloorToInt(Camera.main.transform.position.y - numTilesY / 2);
            for (; y < Mathf.CeilToInt(numTilesY / 2) + 1; y = y+2)
            {
                if (y > -2)
                    break;

                string tileName = "Tile_" + x + "_" + y;
                Vector2 tilePos = new Vector2(x, y);
                if (GameObject.Find(tileName) == null)
                    CreateTile(tilePos, tileName, "Dirt");
            }
        }        
    }

    public void DespawnTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        tiles = tiles.Where(go => (!go.GetComponent<Renderer>().isVisible)).ToArray();

        foreach (GameObject t in tiles)
        {
            SimplePool.Despawn(t);
        }
    }

    void CreateTile(Vector2 pos, string name, string type)
    {
        GameObject prefab = Resources.Load<GameObject>(type);
        // check type exists iin prefabs
        if (prefab == null)
        {
            Debug.LogError("No prefab found: " + type);
        }

        GameObject go = (GameObject)GameObject.Instantiate(prefab);
        //GameObject go = (GameObject)SimplePool.Spawn(prefab, pos, Quaternion.identity);
        go.name = name;
        go.transform.position = pos;
        go.transform.parent = this.transform;

        Tile tile = go.GetComponent<Tile>();
        tile.Pos = pos;
        tile.TileName = name;        
    }

	// Update is called once per frame
	void Update () {
	

	}
}
