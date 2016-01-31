using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

enum questType { buildQuest, followerQuest, lootQuest, summonQuest}

public class QuestManager : MonoBehaviour
{

    private List<Quest> activeQuests;
    private int currentCultists;
    private int currentDemons;
    private int currentGold;
    private float currentRenown;

    public GameObject panel;
    public Text questTextObject;


    public void start()
    {
        currentGold = Camera.main.GetComponent<GameResourceManager>().currentGold();
        currentRenown = Camera.main.GetComponent<GameResourceManager>().currentRenown();
        currentCultists = Camera.main.GetComponent<GameResourceManager>().getCultistsAmt();
        currentDemons = Camera.main.GetComponent<GameResourceManager>().getDemonsAmt();
        activeQuests = new List<Quest>(); // The number here represents the maximum amount of active quests a player can have.
        //Debug.Log("this is called");
    }

    bool lookingAtWindow = false;
    
    public void Update()
    {
        if (activeQuests == null)
            activeQuests = new List<Quest>();

        currentGold = Camera.main.GetComponent<GameResourceManager>().currentGold();
        currentRenown = Camera.main.GetComponent<GameResourceManager>().currentRenown();
        currentCultists = Camera.main.GetComponent<GameResourceManager>().getCultistsAmt();
        currentDemons = Camera.main.GetComponent<GameResourceManager>().getDemonsAmt();
        
        int generationChance = UnityEngine.Random.Range(1, 10801);
        if(generationChance > 0 && !lookingAtWindow)
        {
            Time.timeScale = 0;
            lookingAtWindow = true;
            Quest qu = genRandQuest();
            questTextObject.text = qu.getQuestText();
            panel.SetActive(true);
            activeQuests.Add(qu);           
        }
        lookingAtWindow = panel.activeSelf;
        if (!lookingAtWindow)
            Time.timeScale = 1;        
        
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
        int type = UnityEngine.Random.Range(1, 4);
        int timeLimit = UnityEngine.Random.Range(60, 360);        
        Quest myQuest;
        switch (type)
        {
           // case 0:
               // TileTypes passType = TileTypes.Empty;
               // myQuest = new BuildQuest(ref passType, timeLimit);
               // (myQuest as BuildQuest).StartingNumberOfRoomsOfType = NumberOfRoomsOfType(passType);
              //  break;
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
