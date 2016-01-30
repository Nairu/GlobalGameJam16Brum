using UnityEngine;
using UnityEngine.UI;

public class GameResourceManager : MonoBehaviour {

    private ResourceManager _resourceManager;
    //private GameObject _gameCanvas;
    private Text _goldCounter;
    private Text _soulsCounter;
    private Text _renounCounter;

    public int StartingGold = 0;
    public int StartingSouls = 0;
    public int StartingRenoun = 0;

    void Awake()
    {
        _resourceManager = new ResourceManager(StartingGold, StartingSouls, StartingRenoun);
        _goldCounter = GameObject.FindGameObjectWithTag("Gold").GetComponent<Text>();
        _soulsCounter = GameObject.FindGameObjectWithTag("Souls").GetComponent<Text>();
        _renounCounter = GameObject.FindGameObjectWithTag("Renoun").GetComponent<Text>();
    }

    void Start()
    {
        
    }

    public bool SpendGold(int goldToSpend)
    {
        return _resourceManager.SpendGold(goldToSpend);
    }

    public bool SpendSouls(int soulsToSpend)
    {
        return _resourceManager.SpendSouls(soulsToSpend);
    }

    public bool SpendRenoun(int renounToSpend)
    {
        return _resourceManager.SpendRenoun(renounToSpend);
    }

    public bool SpendResources(int gold = 0, int souls = 0, int renoun = 0)
    {
        return _resourceManager.SpendResources(gold, souls, renoun);
    }

    public void AddGold(int goldToAdd)
    {
        _resourceManager.Gold += goldToAdd;
    }

    public void AddSouls(int soulsToAdd)
    {
        _resourceManager.Souls += soulsToAdd;
    }

    public void AddRenown(int renownToAdd)
    {
        _resourceManager.Renoun += renownToAdd;
    }

    public void Update()
    {
        _goldCounter.text = string.Format("Gold: {0}", _resourceManager.Gold);
        _soulsCounter.text = string.Format("Souls: {0}", _resourceManager.Souls);
        _renounCounter.text = string.Format("Renoun: {0}", _resourceManager.Renoun);
    }
}
