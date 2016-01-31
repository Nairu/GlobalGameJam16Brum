using UnityEngine;
using System.Collections;

public class DemonAI : BaseAI {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Random.Range(1, 10001) > 9990)
        {
            AudioSource youDontKnow = GetComponent<AudioSource>();
            youDontKnow.Play();
        }
    }
}
