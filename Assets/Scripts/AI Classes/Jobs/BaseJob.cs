using UnityEngine;
using System.Collections;

public class BaseJob {
    public Tile tile;
    public BaseAI worker;
    
    public BaseJob(Tile tile)
    {
        this.tile = tile;
        JobQueue.AddJob(this);
        tile.myJob = this as CultistJob;// .myJobs.Add(this);
    }

    public virtual void Done()
    {
        if (tile != null)
            tile.myJob = null;

        if (worker != null)
        {
            worker.myJob = null;
        }

        JobQueue.RemoveJob(this);
    }
}
