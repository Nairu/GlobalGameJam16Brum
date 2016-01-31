using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DoogooderJob : BaseJob {

    public int timeToComplete;
    private Stopwatch watch;

    public DoogooderJob(Tile myJob) : base(myJob)
    {
        watch = new Stopwatch();
        //timeToComplete = myJob.
    }

}
