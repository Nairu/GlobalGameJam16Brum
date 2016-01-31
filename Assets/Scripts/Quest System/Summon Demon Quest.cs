using UnityEngine;
using System.Collections;
using System;

public class SummonDemonQuest : Quest {

    private int noOfDemonsToSummon;

    private int startingNoOfDemons;
    public int StartingNoOfDemons
    {
        get { return startingNoOfDemons; }
        set { startingNoOfDemons = value; }
    }


    public SummonDemonQuest(int timeLimit, int renown) : base(timeLimit) {

        noOfDemonsToSummon = UnityEngine.Random.Range(1, (renown + 1));
        listOfQuests = new string[1];
        listOfQuests[0] = @"Hello there Grand Cult Master
                            . I have a little job for you, if you would be so kind.
                            Please summon for me " + noOfDemonsToSummon + " in " + timeLimit
                            + " seconds. Just need to kill my brother off for the old insurance money, you know how it is.";

    }

    public override bool QuestSucceeded(int currentNoOfDemons)
    {
        return (currentNoOfDemons - startingNoOfDemons >= noOfDemonsToSummon);
    }

}

