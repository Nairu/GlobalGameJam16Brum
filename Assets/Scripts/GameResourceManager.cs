using UnityEngine;

public class GameResourceManager : MonoBehaviour {

    private ResourceManager _resourceManager;

    public int StartingGold = 0;
    public int StartingSouls = 0;
    public int StartingRenoun = 0;

    void Awake()
    {
        _resourceManager = new ResourceManager(StartingGold, StartingSouls, StartingRenoun);
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
}
