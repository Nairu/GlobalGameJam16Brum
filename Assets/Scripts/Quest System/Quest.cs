using UnityEngine;
using System.Collections;
using System.Diagnostics;

public abstract class Quest {

    protected Stopwatch watch;
    protected string questText;
    protected string[] listOfQuests;

    protected int timeLimit;

    protected int rewardGold;
    protected int rewardSouls;
    protected int rewardRenown;

    protected Quest(int timeLimit)
    {
        watch = new Stopwatch();
        this.timeLimit = timeLimit;
    }

    protected void StartQuest()
    {
        watch.Start();
    }

    public bool QuestFailed()
    {
        int elapsedTime = Mathf.RoundToInt(watch.ElapsedMilliseconds / 1000);
        if( elapsedTime == timeLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public abstract bool QuestSucceeded(int i);
    
    public int getGoldReward()
    {
        return rewardGold;
    }

    public int getRenownReward()
    {
        return rewardRenown;
    }

    public int getSoulsReward()
    {
        return rewardSouls;
    }

    public string getQuestText()
    {
        return questText;
    }

}
