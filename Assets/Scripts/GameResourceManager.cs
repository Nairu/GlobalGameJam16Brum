using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameResourceManager : MonoBehaviour {

    private ResourceManager _resourceManager;
    //private GameObject _gameCanvas;
    private Text _goldCounter;
    private Text _soulsCounter;
    private Text _renownCounter;

    public GameObject knight;

    private List<DemonAI> totalDemons;
    private List<CultistAI> totalCultists;
    private List<DoogooderAI> totalDoogooders;

    public int StartingGold = 0;
    public int StartingSouls = 0;
    public int StartingRenoun = 0;

    public float difficultySpeed = 0.5f;

    public float minuteChecker = 0;

    void Awake()
    {
        _resourceManager = new ResourceManager(StartingGold, StartingSouls, StartingRenoun);
        _goldCounter = GameObject.FindGameObjectWithTag("Gold").GetComponent<Text>();
        _soulsCounter = GameObject.FindGameObjectWithTag("Souls").GetComponent<Text>();
        _renownCounter = GameObject.FindGameObjectWithTag("Renoun").GetComponent<Text>();
    }

    void Start()
    {
        
    }

    public bool SpendGold(int goldToSpend)
    {
        return _resourceManager.SpendGold(goldToSpend);
    }

    public int currentGold()
    {
        return _resourceManager.Gold;
    }

    public bool SpendSouls(int soulsToSpend)
    {
        return _resourceManager.SpendSouls(soulsToSpend);
    }

    public bool SpendRenoun(int renounToSpend)
    {
        return _resourceManager.SpendRenoun(renounToSpend);
    }

    public float currentRenown()
    {
        return _resourceManager.Renoun;
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
        // Do not allow soul count to exceed MaxSouls limit, excess souls are wasted.
        if (_resourceManager.Souls + soulsToAdd <= _resourceManager.MaxSouls)
            _resourceManager.Souls += soulsToAdd;
        else
            _resourceManager.Souls = _resourceManager.MaxSouls;
    }

    public void AddRenown(int renownToAdd)
    {
        _resourceManager.Renoun += renownToAdd;
    }


    public void AddDemon()
    {
        _resourceManager.Demons++;
    }

    public int getDemonsAmt()
    {
        return _resourceManager.Demons;
    }

    public void AddCultist()
    {
        _resourceManager.Cultists++;
    }

    public int getCultistsAmt()
    {
        return _resourceManager.Cultists;
    }

    public void Update()
    {
        MapManager map = GameObject.FindObjectOfType<MapManager>();

        if (totalDemons == null)
            totalDemons = new List<DemonAI>();
        if (totalCultists == null)
            totalCultists = new List<CultistAI>();
        if (totalDoogooders == null)
            totalDoogooders = new List<DoogooderAI>();        

    _goldCounter.text = string.Format("Gold: {0}", _resourceManager.Gold);
        _soulsCounter.text = string.Format("Souls: {0}/{1}", _resourceManager.Souls, _resourceManager.MaxSouls);
        _renownCounter.text = string.Format("Renoun: {0}", _resourceManager.Renoun);

        //this.totalDemons.AddRange(GameObject.FindObjectsOfType<DemonAI>().Where(da => (!totalDemons.Contains(da))));
        this.totalCultists.AddRange(GameObject.FindObjectsOfType<CultistAI>().Where(da => (!totalCultists.Contains(da))));
        this.totalDoogooders.AddRange(GameObject.FindObjectsOfType<DoogooderAI>().Where(da => (!totalDoogooders.Contains(da))));

        minuteChecker += Time.deltaTime;
        if (minuteChecker >= 120 * difficultySpeed)
        {
            CultistAI[] array = new CultistAI[Mathf.Abs(map.yDepth / 2)];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = totalCultists[i];
            }

            SpawnKnights.SpawnEnemyKnights(Mathf.Abs(map.yDepth / 2), array, knight);

            minuteChecker = 0;
        }
    }
}
