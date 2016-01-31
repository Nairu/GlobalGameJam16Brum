using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DoogooderAI : BaseAI {

    public int goldDropAmount;
    public int renownGainAmount;
    private Stopwatch watch;
    private int watchCooldown = 1;

    public DoogooderJob myDoogooderJob;
    public int damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Random.Range(1, 10001) > 9990)
        {
            AudioSource youreARascal = GetComponent<AudioSource>();
            youreARascal.Play();
        }

        if (myDoogooderJob == null)
            myDoogooderJob = JobQueue.TakeDoogooderJob(this);
        if (myDoogooderJob != null)
        {
            if (myDoogooderJob.target != null)
            {
                if (((int)myDoogooderJob.target.transform.position.y) != transform.position.y)
                {
                    base.MoveToFloor((int)myDoogooderJob.target.transform.position.y);
                }
                else if (transform.position.x < myDoogooderJob.target.transform.position.x - 0.5f
                        || transform.position.x > myDoogooderJob.target.transform.position.x + 0.5f)
                    base.MoveToX((int)myDoogooderJob.tile.Pos.x);
                else
                {
                    if ((watch.ElapsedMilliseconds / 1000) >= watchCooldown)
                    {
                        Attack();
                        watch.Reset();
                    }
                }
            }
        }
    }

    protected override void Attack()
    {
        if ((transform.position.x - myEnemy.transform.position.x) < 1f)
        {
            myEnemy.DealDamage(damage);            
        }
    }

    protected override void Die()
    {
        Camera.main.GetComponent<GameResourceManager>().AddGold(goldDropAmount);
        Camera.main.GetComponent<GameResourceManager>().AddRenown(renownGainAmount);
    }
}
