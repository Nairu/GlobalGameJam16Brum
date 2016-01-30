using UnityEngine;
using System.Collections;

public class BaseJob {
    public Tile tile;
    public BaseAI worker;
    
    public BaseJob(Tile tile)
    {
        this.tile = tile;
        JobQueue.AddJob(this);
        tile.myJobs.Add(this);
    }

    public virtual void Done()
    {
        if (tile != null)
            tile.myJobs.Remove(this);

        if (worker != null)
        {
            
        }

        JobQueue.RemoveJob(this);
    }
}
