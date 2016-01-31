using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GameObject DormPanel;
    public GameObject summonPanel;
    public GameObject MessHallPanel;
    public GameObject DungeonPanel;
    public GameObject RecreationRoomPanel;
    public GameObject CthuluShrinePanel;

    string OwningTile;

	// Use this for initialization
	void Start () {
        ResetPanels();
    }
	
	// Update is called once per frame
	void Update () {

        if (DormPanel.activeSelf)
            UpdateDormPanel();
        if (MessHallPanel.activeSelf)
            UpdateMessHallPanel();
        if (RecreationRoomPanel.activeSelf)
            UpdateRecreationRoomPanel();

	
	}

    void ResetPanels()
    {
        DormPanel.SetActive(false);
        summonPanel.SetActive(false);
        MessHallPanel.SetActive(false);
        DungeonPanel.SetActive(false);
        RecreationRoomPanel.SetActive(false);
        CthuluShrinePanel.SetActive(false);

    }

    public void DeleteRoom()
    {
        GameObject go = GameObject.Find(OwningTile);
        if (go != null)
        {
            Tile tile = go.GetComponent<Tile>();
            tile.DestroyRoom();
        }
    }

    public void OpenPanel(string tileName, string tiletype)
    {
        if (!string.IsNullOrEmpty(OwningTile))
            ResetPanels();

        switch (tiletype)
        {
            case "SummonRoom":
                OpenSummonerPanel(tileName);
                break;
            case "Dorm":
                OpenDormPanel(tileName);
                break;
            case "MessHall":
                OpenMessHallPanel(tileName);
                break;
            case "Dungeon":
                OpenDungeonPanel(tileName);
                break;
            case "RecreationRoom":
                OpenRecreationRoomPanel(tileName);
                break;
            case "CthuluShrine":
                OpenCthuluShrinePanel(tileName);
                break;
        }
    }


    #region Dorm UI
    void OpenDormPanel(string tileName)
    {
        DormPanel.SetActive(true);
        OwningTile = tileName;
    }
    void UpdateDormPanel()
    {
        Text bedTaken = GameObject.Find("BedsTakenText").GetComponent<Text>();
        MapManager map = GameObject.FindObjectOfType<MapManager>();

        int bedsAvailable = map.dormCount * map.dormValue;
        bedTaken.text = string.Format("Beds : {0}/{1}", 0, bedsAvailable);

    }

    public void CloseDormPanel()
    {
        DormPanel.SetActive(false);
        OwningTile = string.Empty;
    }
    #endregion

    #region SummonRoom UI
    void OpenSummonerPanel(string tileName)
    {
        summonPanel.SetActive(true);
        OwningTile = tileName;
    }
    public void CloseSummonerPanel()
    {
        summonPanel.SetActive(false);
        OwningTile = string.Empty;
    }
    public void SummonDemon()
    {
        Debug.Log("Demon summon started at: " + OwningTile);
    }
    #endregion

    #region Mess Hall UI
    void OpenMessHallPanel(string tileName)
    {
        MessHallPanel.SetActive(true);
        OwningTile = tileName;
    }

    void UpdateMessHallPanel()
    {
        Text TablesTaken = GameObject.Find("TablesTakenText").GetComponent<Text>();
        MapManager map = GameObject.FindObjectOfType<MapManager>();

        int TablesAvailable = map.messCount * map.messValue;
        TablesTaken.text = string.Format("Tables : {0}/{1}", 0, TablesAvailable);

    }

    public void CloseMessHallPanel()
    {
        MessHallPanel.SetActive(false);
        OwningTile = string.Empty;
    }
    #endregion

    #region Dungeon UI
    void OpenDungeonPanel(string tileName)
    {
        DungeonPanel.SetActive(true);
        OwningTile = tileName;
    }
    public void CloseDungeonPanel()
    {
        DungeonPanel.SetActive(false);
        OwningTile = string.Empty;
    }
    #endregion

    #region RecreationRoom UI
    void OpenRecreationRoomPanel(string tileName)
    {
        RecreationRoomPanel.SetActive(true);
        OwningTile = tileName;
    }

    void UpdateRecreationRoomPanel()
    {
        Text capacityFilled = GameObject.Find("CapacityFilledText").GetComponent<Text>();
        MapManager map = GameObject.FindObjectOfType<MapManager>();

        int capacityAvailable = map.recreationCount * map.recreationValue;
        capacityFilled.text = string.Format("Capacity : {0}/{1}", 0, capacityAvailable);

    }

    public void CloseRecreationRoomPanel()
    {
        RecreationRoomPanel.SetActive(false);
        OwningTile = string.Empty;
    }
    #endregion

    #region CthuluShrine UI
    void OpenCthuluShrinePanel(string tileName)
    {
        CthuluShrinePanel.SetActive(true);
        OwningTile = tileName;
    }

    public void BeginTheRitual()
    {
        Debug.Log("Begin the Ritual to summon forth Cthulu");
    }

    public void CloseCthuluShrinePanel()
    {
        CthuluShrinePanel.SetActive(false);
        OwningTile = string.Empty;
    }
    #endregion
}
