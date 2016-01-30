using UnityEngine;
using System.Collections;

public class CameraMoveController : MonoBehaviour {

    private Transform myTransform;
    private Camera myCamera;

    public MapManager map;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<Transform>();
        myCamera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 2, -10);
            map.SpawnTiles();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x + 3, myTransform.position.y, -10);
            map.SpawnTiles();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x - 3, myTransform.position.y, -10);
            map.SpawnTiles();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y + 2, -10);
            map.SpawnTiles();
        }
    }

    public Vector2 AbsolutePositionUnerMouse()
    {
        Vector3 checkVector = myCamera.ScreenToWorldPoint(Input.mousePosition);

        return new Vector2(Mathf.Floor(checkVector.x), Mathf.FloorToInt(checkVector.y));
    }
}
