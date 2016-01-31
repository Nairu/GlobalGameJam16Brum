using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DoogooderJob : BaseJob {

    public int timeToComplete;
    private Stopwatch watch;
    public CultistAI target;

    public DoogooderJob(Tile myJob, CultistAI target) : base(myJob)
    {
        watch = new Stopwatch();
        
    }

}
