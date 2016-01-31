using UnityEngine;
using System.Collections;

public class CultistAI : BaseAI {

    public float needThreshold = 0.5f;
    public float needBadly = 1f;
    public float needHardLimit = 3f;
    public new float cultistSpeed;

    public float needSleep = 0;
    public float needFood = 0;
    public float needFun = 0;

    public CultistJob myCultistJob;
    public Tile myCultistCareer;

    public new int Health
    {
        get { return base.Health; }
    }

    public void Update()
    {
        needSleep += Time.deltaTime / 143f;
        needFood += Time.deltaTime / 92f;
        needFun += Time.deltaTime / 125f;

        if (needSleep >= needHardLimit || needFood >= needHardLimit || needFun >= needHardLimit)
        {
            base.Die();
        }

        if (NeedSleep())
        {
            if (myJob != null)
                myJob.Done();
            return;
        }

        if (FunFoodNeeds())
        {
            if (myJob != null)
                myJob.Done();
            return;
        }
        
        DoWander();
    }

    NeedProvider GetClosestProvider(NeedType need)
    {
        NeedProvider closest = null;
        float dist = 0;

        NeedProvider[] provs = GameObject.FindObjectsOfType<NeedProvider>();
        foreach (NeedProvider p in provs)
        {
            if (p.need == need && !p.IsFull())
            {
                Tile t = p.GetComponent<Tile>();

                float d = Mathf.Abs(t.Pos.y - transform.position.y);
                if (closest == null || d < dist)
                {
                    closest = p;
                    dist = d;
                }

                if (p.myCultists.Contains(this))
                    p.myCultists.Remove(this);
            }
        }

        return closest;
    }

    float eatTimer;
    float sleepTimer;
    float funTimer;

    bool FunFoodNeeds()
    {
        Tile t;

        if (needFood > needThreshold && needFood >= needFun)
        {
            Debug.Log(this.name + " Needs Food");

            NeedProvider n = GetClosestProvider(NeedType.Food);
            if (n != null)
            {
                t = n.GetComponent<Tile>();
                if (t.Pos.y != transform.position.y)
                {
                    //Go to floor                    
                    base.MoveToFloor(t.Pos.y, transform.position.x);
                }
                else if (n.myPositions[n.myCultists.Count].position.x 
                         != (Mathf.RoundToInt(transform.position.x / 3) * 3))
                {
                    if (!n.myCultists.Contains(this))
                        n.myCultists.Add(this);
                    base.MoveToX(n.myPositions[n.myCultists.Count - 1].position.x);

                    //Go to position
                }
                else
                {
                    if (eatTimer <= 0)
                    {
                        //set eatTimer = some abstract length
                        eatTimer = 1;
                    }

                    eatTimer -= Time.deltaTime;
                    //set animation
                    if (eatTimer <= 0)
                    {
                        needFood = 0;
                        if (n.myCultists.Contains(this))
                            n.myCultists.Remove(this);
                        return false;
                    }                   
                    return true;
                }
            }
        }

        if (needFun > needThreshold && needFun >= needFood)
        {
            Debug.Log(this.name + " Needs Fun");

            NeedProvider p = GetClosestProvider(NeedType.Fun);
            if (p != null)
            {
                t = p.GetComponent<Tile>();
                if (t.Pos.y != transform.position.y)
                {
                    base.MoveToFloor(t.Pos.y, transform.position.x);
                }
                else if (p.myPositions[p.myCultists.Count].position.x 
                         != (Mathf.RoundToInt(transform.position.x / 3) * 3))
                {
                    if (!p.myCultists.Contains(this))
                        p.myCultists.Add(this);
                    base.MoveToX(p.myPositions[p.myCultists.Count - 1].position.x);
                }
                else
                {
                    
                    if (funTimer <= 0)
                    {
                        // some arbitrary length
                        funTimer = 1;
                    }

                    funTimer -= Time.deltaTime;
                    //set the animation
                    if (funTimer <= 0)
                    {
                        needFun = 0;
                        if (p.myCultists.Contains(this))
                            p.myCultists.Remove(this);
                        return false;
                    }
                    return true;
                }
            }
        }

        return false;        
    }

    bool sleeping = false;

    bool NeedSleep()
    {
        Tile t = null;
        NeedProvider p = GetClosestProvider(NeedType.Sleep);

        if (sleeping)
        {
            t = p.GetComponent<Tile>();
            if (sleepTimer <= 0)
            {
                needSleep = 0;
                
                return false;
            }
        }
        else
        {
            if (needSleep > needThreshold)
            {
                if (p != null)
                {
                    t = p.GetComponent<Tile>();
                    if (t.Pos.y != transform.position.y)
                    {
                        base.MoveToFloor(t.Pos.y, transform.position.x);
                    }
                    else if (t.Pos.x != (Mathf.RoundToInt(transform.position.x / 3) * 3))
                    {
                        base.MoveToX(t.Pos.x);
                    }
                    else
                    {
                        DoSleep();
                        
                        return true;
                    }
                }
            }
        }

        if (p != null)
        {
            if (p.myCultists.Contains(this))
                p.myCultists.Remove(this);
        }
        return false;
    }

    void DoSleep()
    {
        if (myJob != null)
        {
            myJob.Done();
            myJob = null;
        }
    }

    float wanderTime = 0;
    int wanderDirection;

    void DoWander()
    {
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0)
        {
            wanderDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            wanderTime = Random.Range(0.5f, 1.0f);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z);

        GetComponent<Animator>().SetBool("Walking", true);

        float tryX = ((int)Mathf.Round(transform.position.x / 3) * 3) + cultistSpeed * wanderDirection * Time.deltaTime;
        Tile t = base.myMap.GetTileAt(Mathf.RoundToInt(tryX), (int)transform.position.y);
        if (t != null && !t.isWalkable)
        {
            wanderDirection *= -1;
        }
        else
        {
            Vector3 dir = new Vector3(cultistSpeed / 2 * wanderDirection, 0, 0) * Time.deltaTime;
            base.FaceToMovement(dir);
            transform.Translate(dir);
        }
    }
}
