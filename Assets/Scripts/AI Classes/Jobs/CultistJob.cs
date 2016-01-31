using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CultistJob : BaseJob {

    public int timeToComplete;
    private Stopwatch watch;

	public CultistJob(Tile myJob) : base(myJob)
    {
        watch = new Stopwatch();
        //timeToComplete = myJob.
    }

    public void StartJob()
    {
        if (watch == null)
            watch = new Stopwatch();
        watch.Start();
    }

    public void KillJob()
    {
        watch.Reset();
    }

    public void StopJob()
    {
        watch.Stop();
    }

    public override void Done()
    {
        if (tile != null)
            tile.myJob = null;

        if (worker != null)
        {
            (worker as CultistAI).myCultistJob = null;
            (worker as CultistAI).myJob = null;
        }

        JobQueue.RemoveJob(this);
    }

    public bool JobCompleted()
    {
        return (watch.ElapsedMilliseconds / 1000) > timeToComplete; //For if we work in seconds with our timeToComplete
    }
}
