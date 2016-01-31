﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

enum questType { buildQuest, followerQuest, lootQuest, summonQuest}

public class QuestManager
{

    private List<Quest> activeQuests;
    private int currentCultists;
    private int currentDemons;
    private int currentGold;
    private int currentRenown;



    public QuestManager(int currentCultists, int currentDemons, int currentGold, int currentRenown)
    {
        this.currentCultists = currentCultists; this.currentDemons = currentDemons; this.currentGold = currentGold; this.currentRenown = currentRenown;
        activeQuests = new List<Quest>(5); // The number here represents the maximum amount of active quests a player can have.
    }


    public void runQuestManager(int currentCultists, int currentDemons, int currentGold, int currentRenown)
    {
        this.currentCultists = currentCultists; this.currentDemons = currentDemons; this.currentGold = currentGold; this.currentRenown = currentRenown;
        int generationChance = UnityEngine.Random.Range(1, 10801);
        if(generationChance == 10800)
        {
            activeQuests.Add(genRandQuest());
        }

        foreach (Quest q in activeQuests)
        {
            int valueToPass = 1;
            switch (q.GetType().ToString())
            {
                case "BuildQuest":
                    //valueToPass = currentNumberOfRoomsOfType -.-
                    break;
                case "LootQuest":
                    valueToPass = currentGold;
                    break;
                case "GetFollowerQuest":
                    valueToPass = currentCultists;
                    break;
                case "SummonDemonQuest":
                    valueToPass = currentDemons;
                    break;
                default:
                    throw new System.Exception("Wrong type of quest");
            }

            if (q.QuestFailed())
            {
                activeQuests.Remove(q);
                // Bring up quest failed diaglog?
            }
            else if (q.QuestSucceeded(valueToPass))
            {
                activeQuests.Remove(q);
                // Quest Succeeded dialog
                // Give player reward for completing quest
            }
        }       
    }

    private Quest genRandQuest()
    {
        int type = UnityEngine.Random.Range(0, 4);
        int timeLimit = UnityEngine.Random.Range(60, 360);        
        Quest myQuest;
        switch (type)
        {
            case 0:
                TileTypes passType = TileTypes.Empty;
                myQuest = new BuildQuest(ref passType, timeLimit);
                (myQuest as BuildQuest).StartingNumberOfRoomsOfType = NumberOfRoomsOfType(passType);
                break;
            case 1:
                myQuest = new GetFollowerQuest(timeLimit, currentRenown);
                break;
            case 2:
                myQuest = new LootQuest(timeLimit, currentRenown);
                break;
            case 3:
                myQuest = new SummonDemonQuest(timeLimit, currentRenown);
                break;
            default:
                throw new UnityException("A quest could not be generated");
        }

        return myQuest;
    }

    private int NumberOfRoomsOfType(TileTypes type)
    {
        return GameObject.FindObjectsOfType<Tile>().Where(t => (t.TileType == Enumerations.GetEnumDescription(type))).ToArray().Length;
    }

    public string getQuestText(int questNum)
    {
        return activeQuests[questNum].getQuestText();
    }

}
