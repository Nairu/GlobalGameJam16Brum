using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

    private int _gold;
    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    
    private int _souls;
    public int Souls
    {
        get { return _souls; }
        set { _souls = value; }
    }

    private float _renown;
    public float Renown
    {
        get { return _renown; }
        set { _renown = value; }
    }

    
}
