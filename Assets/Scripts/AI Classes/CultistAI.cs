using UnityEngine;
using System.Collections;

public class CultistAI : BaseAI {
    
    public float needThreshold = 0.5f;
    public float needBadly = 1f;
    public float needHardLimit = 3f;
    public new float cultistSpeed;

    public int myHash;
    public Transform boundTransform;
    public Animator myAnimator;

    bool moving = false;
    bool working = false;

    public float needSleep = 0;
    public float needFood = 0;
    public float needFun = 0;

    [SerializeField()]
    public CultistJob myCultistJob;
    public Tile myCultistCareer;

    void Awake()
    {        
        myAnimator = GetComponent<Animator>();
    }

    public new int Health
    {
        get { return base.Health; }
    }

    public void Update()
    {
        myAnimator.SetBool("Walking", false);
        myAnimator.SetBool("Worshipping", false);
        myAnimator.SetBool("Climbing", false);
        myAnimator.SetBool("Praying", false);        

        myHash = GetHashCode();
        if (!sleeping)
            needSleep += Time.deltaTime / 150f;
        else
            needSleep -= Time.deltaTime / 50f;
        needFood += Time.deltaTime / 92f;
        needFun += Time.deltaTime / 125f;

        if (UnityEngine.Random.Range(1, 10001) > 9990)
        {
            AudioSource wololo = GetComponent<AudioSource>();
            wololo.Play();
        }

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

        else if (FunFoodNeeds())
        {
            if (myJob != null)
                myJob.Done();
            return;
        }

        DoWork();

        if (!moving && !working)
            DoWander();
    }

    NeedProvider GetClosestProvider(NeedType need)
    {
        NeedProvider closest = null;
        float dist = 0;

        NeedProvider[] provs = GameObject.FindObjectsOfType<NeedProvider>();
        foreach (NeedProvider p in provs)
        {
            if (p.need == need && !p.IsFull() && !p.myCultists.Contains(this))
            {
                Tile t = p.GetComponent<Tile>();

                float d = Mathf.Abs(t.Pos.y - transform.position.y);
                if (closest == null || d < dist)
                {
                    closest = p;
                    dist = d;
                }
            }
            else if (p.myCultists.Contains(this))
            {
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

        if (needSleep > needThreshold)
            return true;

        if (needFood > needThreshold && needFood >= needFun)
        {
            Debug.Log(this.name + " Needs Food");

            NeedProvider n = GetClosestProvider(NeedType.Food);
            if (n != null)
            {
                t = n.GetComponent<Tile>();
                if (t.Pos.y != transform.position.y)
                {
                    moving = true;
                    //Go to floor                    
                    base.MoveToFloor(t.Pos.y);
                }
                else if (transform.position.x < t.Pos.x ||
                          transform.position.x > t.Pos.x + 3)
                {
                    moving = true;
                    if (!n.myCultists.Contains(this))
                    {
                        //myMap.messCount++;
                        n.myCultists.Add(this);
                    }

                    for (int i = 0; i < n.myCultists.Count; i++)
                    {                        
                        n.myCultists[i].MoveToX(n.myPositions[i].transform.position.x);
                    }

                    //Go to position
                }
                else
                {
                    moving = false;
                    if (eatTimer <= 0)
                    {
                        //set eatTimer = some abstract length
                        eatTimer = n.timeToWait;
                    }

                    eatTimer -= Time.deltaTime;
                    //set animation
                    if (eatTimer <= 0)
                    {
                        needFood = 0;
                        if (n.myCultists.Contains(this))
                        {
                            //myMap.messCount--;
                            n.myCultists.Remove(this);
                        }
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
                    moving = true;
                    base.MoveToFloor(t.Pos.y);
                }
                else if (transform.position.x < t.Pos.x
                         || transform.position.x > t.Pos.x + 3)
                {
                    moving = true;
                    if (!p.myCultists.Contains(this))
                    {
                        //myMap.recreationCount++;
                        p.myCultists.Add(this);
                    }

                    for (int i = 0; i < p.myCultists.Count; i++)
                    {
                        p.myCultists[i].MoveToX(p.myPositions[i].transform.position.x);
                    }
                }
                else
                {
                    moving = false;
                    if (funTimer <= 0)
                    {
                        // some arbitrary length
                        funTimer = p.timeToWait;
                    }

                    funTimer -= Time.deltaTime;
                    //set the animation
                    if (funTimer <= 0)
                    {
                        needFun = 0;
                        if (p.myCultists.Contains(this))
                        {
                            //myMap.recreationCount--;
                            p.myCultists.Remove(this);
                        }
                        return false;
                    }
                    return true;
                }
            }
        }

        return false;        
    }

    bool sleeping = false;

    bool DoWork()
    {
        if (myCultistJob == null)
            myCultistJob = JobQueue.TakeCultistJob(this);
        if (myCultistJob == null)
            return false;

            if (myCultistJob.tile != null)
        {
            if (myCultistJob.tile.TileType == Enumerations.GetEnumDescription(TileTypes.SummonRoom))
            {
                Debug.Log("Cultist: " + (int)myCultistJob.tile.Pos.y + " | " + "Target: " + transform.position.y);
                if ((int)myCultistJob.tile.Pos.y != transform.position.y)
                {
                    moving = true;
                    base.MoveToFloor((int)myCultistJob.tile.Pos.y);
                }
                else if (transform.position.x < myCultistJob.tile.Pos.x + 0.9f
                        || transform.position.x > myCultistJob.tile.Pos.x + 1.1f)
                    base.MoveToX((int)myCultistJob.tile.Pos.x + 1f);
                else
                {
                    myCultistJob.timeToComplete = myCultistJob.tile.JobDuration;

                    GetComponent<Animator>().SetBool("Praying", true);
                    GetComponent<Animator>().SetBool("Walking", false);

                    myCultistJob.tile.myJob = myCultistJob;
                    myCultistJob.StartJob();
                    working = true;
                }
            }
        }

        return false;
        //if ()
    }

    bool NeedSleep()
    {
        Tile t = null;
        NeedProvider p = GetClosestProvider(NeedType.Sleep);

        if (sleeping)
        {
            t = p.GetComponent<Tile>();
           // if (sleepTimer <= 0)
           if (needSleep < 0)
            {
                //needSleep = 0;

                sleeping = false;
                return false;
            }
            return true;
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
                        moving = true;
                        base.MoveToFloor(t.Pos.y);
                    }
                    else if (t.Pos.x != (Mathf.RoundToInt(transform.position.x / 3) * 3))
                    {
                        moving = true;
                        base.MoveToX(t.Pos.x);
                    }
                    else
                    {
                        moving = false;
                        sleeping = true;
                        DoSleep();
                        
                        return true;
                    }
                }
                return true;
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
