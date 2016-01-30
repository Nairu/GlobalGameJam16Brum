using UnityEngine;
using System.Collections;
using System.Linq;

public class MapManager : MonoBehaviour {

    public int numTilesY;    
    public int numTilesX;


    public float RemTestX;
    public float RemTestY;
    public float tileX;
    public float tileY;
    public GameObject goTest;

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

    public void CreateTile(Vector2 pos, string name, string type)
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
        tile.TileType = type;      
    }

    public void ChangeTile(Vector2 pos, string type)
    {
        Debug.Log("changing : Tile_" + pos.x + "_" + pos.y);
        string tileName = "Tile_" + pos.x + "_" + pos.y;
        GameObject go = (GameObject)GameObject.Find(tileName);
        Tile targetTile = go.GetComponent<Tile>();
        Destroy(targetTile.gameObject);

        CreateTile(pos, tileName, type);
    }

	// Update is called once per frame
	void Update () {
	
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.GetComponent<CameraMoveController>().AbsolutePositionUnerMouse();
            Debug.Log(mousePos);

            //goTest = ClickSelect();

            //Destroy(goTest);

        }
	}

    GameObject ClickSelect()
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 12.0f);

        if (hit)
        {
            Debug.Log(hit.transform.name);
            return hit.transform.gameObject;
        }
        else return null;
    }

    float MakePositive(float input)
    {
        if (input < 0)
            return input * -1;
        else
            return input;
    }
}
