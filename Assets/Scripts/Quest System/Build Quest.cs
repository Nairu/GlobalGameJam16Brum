using UnityEngine;
using System.Collections;
using System;

enum roomTypes { BunkRoom, MessHall, SummoningRoom, PrisonRoom};

public class BuildQuest : Quest {

    TileTypes roomType = TileTypes.Empty;
    private int numOfRoomsToBuild = 0;
    private int startingNumberOfRoomsOfType;
    public int StartingNumberOfRoomsOfType
    {
        get { return startingNumberOfRoomsOfType; }
        set { startingNumberOfRoomsOfType = value; }
    }

    public BuildQuest(ref TileTypes passbackRoomType, int timeLimit) : base(timeLimit)
    {        
        
        listOfQuests = new string[1];
        string roomToBuild = getRoom();
        listOfQuests[0] = @"I bid you good tidings my friends. I bear grave news of an attack that is being plotted 
                            against you by the foul knights. You must expand your lair to prepare for their coming! 
                            I implore you to build " + roomToBuild + " You have " + timeLimit + " seconds! Get to it!";
        questText = listOfQuests[UnityEngine.Random.Range(0, listOfQuests.Length)];
        passbackRoomType = roomType;
    }

    public override bool QuestSucceeded(int currentNumberOfRoomsOfType)
    {
        return (currentNumberOfRoomsOfType > StartingNumberOfRoomsOfType);
    }

    private string getRoom()
    {
        int type = UnityEngine.Random.Range(0, 4);
        numOfRoomsToBuild = UnityEngine.Random.Range(1, 4);
        switch (type)
        {
            case 0:
                roomType = TileTypes.Dorm;
                return "a bunk room.";
            case 1:
                roomType = TileTypes.MessHall;
                return "a mess hall.";
            case 2:
                roomType = TileTypes.SummonRoom;
                return "a summoning room.";
            case 3:
                roomType = TileTypes.Dungeon;
                return "a prison room.";
            default: return "fucking nothing because the code is fucked!. ";
        }


    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
