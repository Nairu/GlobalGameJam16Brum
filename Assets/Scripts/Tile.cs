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
	
	// Update is called once per frame
	void Update () {
	
	}
}
