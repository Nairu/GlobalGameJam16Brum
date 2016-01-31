using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DemonJob : BaseJob {

    public int timeToComplete;
    private Stopwatch watch;

    public DemonJob(Tile myJob) : base(myJob)
    {
        watch = new Stopwatch();
        //timeToComplete = myJob.
    }

}
