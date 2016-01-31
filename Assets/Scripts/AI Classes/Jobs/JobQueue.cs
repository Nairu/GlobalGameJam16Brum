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

    public static void AddDemonJob(DemonJob j)
    {
        InitOpenJobs();
        openDemonJobs.AddLast(j);
    }

    public static void AddGoodyJob(DoogooderJob j)
    {
        InitOpenJobs();
        openDoogooderJobs.AddLast(j);
    }

    public static void RemoveCultistJob(CultistJob j)
    {
        openCultistJobs.Remove(j);
    }
    
    public static CultistJob TakeCultistJob(CultistAI worker)
    {
        InitOpenJobs();
        if (openCultistJobs.Count == 0)
            return null;

        if (worker.myCultistJob != null)
            return null;
        
        CultistJob j = (openCultistJobs.First as LinkedListNode<CultistJob>).Value;
        while (j.worker != null)
        {
            openCultistJobs.RemoveFirst();
            j = (openCultistJobs.First as LinkedListNode<CultistJob>).Value;
        }

        j.worker = worker;
        openCultistJobs.RemoveFirst();
        return j;
    }

    public static DemonJob TakeDemonJob(DemonAI worker)
    {
        InitOpenJobs();
        if (openDemonJobs.Count == 0)
            return null;

        if (worker.myJob != null)
            return null;

        DemonJob j = (openDemonJobs.First as LinkedListNode<DemonJob>).Value;
        while (j.worker != null)
        {
            openDemonJobs.RemoveFirst();
            j = (openDemonJobs.First as LinkedListNode<DemonJob>).Value;
        }

        j.worker = worker;
        openDemonJobs.RemoveFirst();
        return j;
    }

    public static DoogooderJob TakeDoogooderJob(DoogooderAI worker)
    {
        InitOpenJobs();
        if (openDoogooderJobs.Count == 0)
            return null;

        if (worker.myJob != null)
            return null;

        DoogooderJob j = (openDoogooderJobs.First as LinkedListNode<DoogooderJob>).Value;
        while (j.worker != null)
        {
            openDemonJobs.RemoveFirst();
            j = (openDoogooderJobs.First as LinkedListNode<DoogooderJob>).Value;
        }

        j.worker = worker;
        openDemonJobs.RemoveFirst();
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
