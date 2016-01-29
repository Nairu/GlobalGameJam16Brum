using UnityEngine;
using System.Collections;

public class CameraMoveController : MonoBehaviour {

    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 2, 0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x + 2, myTransform.position.y, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x - 2, myTransform.position.y, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y + 2, 0);
        }
    }
}
