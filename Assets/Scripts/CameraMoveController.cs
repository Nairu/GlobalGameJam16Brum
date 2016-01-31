using UnityEngine;
using System.Collections;

public class CameraMoveController : MonoBehaviour {

    private Transform myTransform;
    private Camera myCamera;

    public MapManager map;
    public int cameraLimitX;

    public int RoomToPlace = 3;
    public int GoldCost = 0;
    public int SoulsCost = 0;

    // Use this for initialization
    void Start () {
        myTransform = GetComponent<Transform>();
        myCamera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 2, -10);
            map.SpawnTiles();
            //map.DespawnTiles();
        }
        else if((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && myTransform.position.x < cameraLimitX )
        {
            myTransform.position = new Vector3(myTransform.position.x + 3, myTransform.position.y, -10);
            map.SpawnTiles();
            //map.DespawnTiles();
        }
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && myTransform.position.x > (-1 * cameraLimitX + 3))
        {
            myTransform.position = new Vector3(myTransform.position.x - 3, myTransform.position.y, -10);
            map.SpawnTiles();
            //map.DespawnTiles();
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && myTransform.position.y < 5)
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y + 2, -10);
            map.SpawnTiles();
            //map.DespawnTiles();
        }
    }

    public Vector2 AbsolutePositionUnerMouse()
    {
        Vector3 checkVector = myCamera.ScreenToWorldPoint(Input.mousePosition);

        return new Vector2(Mathf.Floor(checkVector.x), Mathf.FloorToInt(checkVector.y));
    }

    public void SetRoom(int roomToSet)
    {
        RoomToPlace = roomToSet;
        Tile refTile = map.GetTileArchetype(Enumerations.GetEnumDescription((TileTypes)roomToSet));        
        GoldCost = refTile.goldCost;        
        SoulsCost = refTile.soulsCost;
    }
}
