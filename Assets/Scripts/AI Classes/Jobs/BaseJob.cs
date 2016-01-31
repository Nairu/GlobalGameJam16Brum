using UnityEngine;
using System.Collections;

public class BaseJob {
    public Tile tile;
    public BaseAI worker;
    
    public BaseJob(Tile tile)
    {
        
    }

    public virtual void Done()
    {
        if (tile != null)
            tile.myJob = null;

        if (worker != null)
        {
            worker.myJob = null;
        }

        //JobQueue.RemoveJob(this);
    }
}
