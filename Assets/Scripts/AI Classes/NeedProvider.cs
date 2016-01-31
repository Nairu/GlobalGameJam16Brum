using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NeedType
{
    Food = 0,
    Sleep,
    Fun
}

public class NeedProvider : MonoBehaviour
{
    public NeedType need;
    public int capacity;
    public Transform[] myPositions;
    public List<CultistAI> myCultists;

    public float timeToWait = 1f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CultistAI cai = collision.GetComponent<CultistAI>();
        if (cai != null)
        {

        }
    }

    public bool IsFull()
    {
        return myCultists.Count == capacity;
    }
}
