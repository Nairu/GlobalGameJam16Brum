using UnityEngine;
using System.Collections;
using System;

public class LootQuest : Quest {

    private int lootToget;

    private int startingLoot;
    public int StartingLoot
    {
        set { startingLoot = value; }
        get { return startingLoot; }
    }

    public LootQuest(int timeLimit, int renown) : base(timeLimit){

        lootToget = UnityEngine.Random.Range(1, (renown * 10) + 1);
        listOfQuests = new string[1];
        listOfQuests[0] = @"Hello travller, it's me, the potion seller.
                            As you know, I only sell the strongest of potions, and your cultists cannot handle my potions
                            but I do know there is a village nearby with some valuable weaker potions worth: " + lootToget*2 + " gold." +
                            " If you get them for me within " + timeLimit + " seconds, I'll split the loot with you";

    }

    public override bool QuestSucceeded(int currentLoot)
    {
        return (currentLoot - startingLoot >= lootToget);
    }

    
}
