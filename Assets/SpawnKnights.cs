using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class SpawnKnights : MonoBehaviour {

    public static GameObject Knight;
    public static Transform spawn;
    private static Stopwatch watch;
    private int spawnDelay;

    public static void SpawnEnemyKnights(int numberToSpawn, CultistAI[] cultists, GameObject knight)
    {
        if (watch == null)
            watch = new Stopwatch();

        watch.Start();

        for (int i = 0; i < numberToSpawn; i++)
        {
            while ((watch.ElapsedMilliseconds / 1000) < 5)
            {

            }
            GameObject.Instantiate(knight, spawn.position, Quaternion.identity);
            JobQueue.AddGoodyJob(new DoogooderJob(null, cultists[i]));
            //JobQueue.Add
        }

        watch.Reset();
    }

	// Use this for initialization
	void Start () {
        spawn = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
