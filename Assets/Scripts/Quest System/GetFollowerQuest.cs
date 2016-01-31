using UnityEngine;
using System.Collections;
using System;

public class GetFollowerQuest : Quest {


    private int followersToAquire;

    private int startingNumberOfFollowers;
    public int StartingNumberOfFollowers
    {
        get { return startingNumberOfFollowers; }
        set { startingNumberOfFollowers = value; }
    }


    public GetFollowerQuest(int timeLimit, float renown) : base(timeLimit){

        followersToAquire = UnityEngine.Random.Range(1, Mathf.RoundToInt(renown) + 1);
        listOfQuests = new string[1];
        listOfQuests[0] = @"Hmmmmm, your cult is looking a wee bit puny is it not?
                            I think you'd benefit if you had a few more fellows join you.
                            I'm a generous chap, and I'd like to see you all do well,
                            so I'll give you something if you can get " + followersToAquire + 
                            " chaps to sign up to your cause in " + timeLimit + " seconds.";
        questText = listOfQuests[UnityEngine.Random.Range(0, listOfQuests.Length)];


    }
        
    public override bool QuestSucceeded(int currentNumberOfFollowers)
    {
        return ((currentNumberOfFollowers - StartingNumberOfFollowers) >= followersToAquire);
    }

    public int getFollowersToGet()
    {
        return followersToAquire;
    }

}
