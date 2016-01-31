using UnityEngine;
using System.Collections;

public class DoogooderAI : BaseAI {

    public int goldDropAmount;
    public int renownGainAmount;

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
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Die()
    {
        Camera.main.GetComponent<GameResourceManager>().AddGold(goldDropAmount);
        Camera.main.GetComponent<GameResourceManager>().AddRenown(renownGainAmount);
    }
}
