using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobQueue : MonoBehaviour {

    //static List<BaseJob> openJobs;
    static LinkedList<BaseJob> openJobs;

    public static void AddJob(BaseJob j)
    {
        InitOpenJobs();
        openJobs.AddLast(j);
    }

    public static void RemoveJob(BaseJob j)
    {
        openJobs.Remove(j);
    }

    public static BaseJob TakeJob(BaseAI worker)
    {
        InitOpenJobs();
        if (openJobs.Count == 0)
            return null;

        if ((worker as CultistAI).myCultistJob != null)
            return null;

        int i = 0;
        while (i < openJobs.Count)
        {
            BaseJob j = (openJobs.First as LinkedListNode<BaseJob>).Value;

            if (worker.GetType() == typeof(CultistAI) && j.worker == null)
            {
                if (j.tile.IsLadderReachable() != -1)
                {                    
                    j.worker = worker;
                    openJobs.Remove(j);
                    return j as CultistJob;
                }
            }
            else if(worker.GetType() == typeof(DemonAI))
            {

            }
            else if(worker.GetType() == typeof(DoogooderAI))
            {

            }
            i++;
        }

        return null;
    }

    public static void MaxPriority(BaseJob j)
    {
        if (openJobs.Contains(j))
        {
            openJobs.Remove(j);
        }
        openJobs.AddFirst(j);
    }

    static void InitOpenJobs()
    {
        if (openJobs == null)
            openJobs = new LinkedList<BaseJob>();
    }

    void Start()
    {
        InitOpenJobs();
    }
}
