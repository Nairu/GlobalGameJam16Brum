using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobQueue : MonoBehaviour {

    //static List<BaseJob> openJobs;
    static LinkedList<CultistJob> openCultistJobs;
    static LinkedList<DemonJob> openDemonJobs;
    static LinkedList<DoogooderJob> openDoogooderJobs;    

    public static void AddCultistJob(CultistJob j)
    {
        InitOpenJobs();
        openCultistJobs.AddLast(j);
    }

    public static void RemoveCultistJob(CultistJob j)
    {
        openCultistJobs.Remove(j);
    }

    //public static bool TakeCultistJob(CultistAI worker)
    //{
    //    InitOpenJobs();
    //    if (openJobs.Count == 0)
    //        return false;

    //    int i = 0;
    //    for (int j = 0; j < AIs.Count; j++)
    //    {
    //        if (AIs[j].myJob != null)
    //            continue;

    //        while (i < openJobs.Count)
    //        {
    //            BaseJob job = (openJobs.First as LinkedListNode<BaseJob>).Value;

    //            if (AIs[j].GetType() == typeof(CultistAI) && job.worker == null)
    //            {
    //                if (job.tile.IsLadderReachable() != -1)
    //                {
    //                    job.worker = AIs[j];
    //                    AIs[j].myJob = job;
    //                    GameObject.Find(AIs[j].myName).GetComponent<CultistAI>().myCultistJob = job as CultistJob;

    //                    //openJobs.Remove(job);                        
    //                    openJobs.RemoveFirst();
    //                    return true;
    //                }
    //            }
    //            else if (AIs[j].GetType() == typeof(DemonAI))
    //            {

    //            }
    //            else if (AIs[j].GetType() == typeof(DoogooderAI))
    //            {

    //            }
    //            i++;
    //        }
    //        j++;
    //    }

    //    return false;
    //}

    public static CultistJob TakeCultistJob(CultistAI worker)
    {
        InitOpenJobs();
        if (openCultistJobs.Count == 0)
            return null;

        if (worker.myCultistJob != null)
            return null;

        CultistJob j = (openCultistJobs.First as LinkedListNode<CultistJob>).Value;
        j.worker = worker;
        openCultistJobs.RemoveFirst();
        return j;
    }

    public static void MaxCultistPriority(CultistJob j)
    {
        if (openCultistJobs.Contains(j))
        {
            openCultistJobs.Remove(j);
        }
        openCultistJobs.AddFirst(j);
    }

    public static void MaxDemonPriority(DemonJob j)
    {
        if (openDemonJobs.Contains(j))
        {
            openDemonJobs.Remove(j);
        }
        openDemonJobs.AddFirst(j);
    }

    public static void MaxDoogooderPriority(DoogooderJob j)
    {
        if (openDoogooderJobs.Contains(j))
        {
            openDoogooderJobs.Remove(j);
        }
        openDoogooderJobs.AddFirst(j);
    }

    static void InitOpenJobs()
    {
        if (openCultistJobs == null)
            openCultistJobs = new LinkedList<CultistJob>();
        if (openDemonJobs == null)
            openDemonJobs = new LinkedList<DemonJob>();
        if (openDoogooderJobs == null)
            openDoogooderJobs = new LinkedList<DoogooderJob>();        
    }

    void Start()
    {
        InitOpenJobs();
    }
}
