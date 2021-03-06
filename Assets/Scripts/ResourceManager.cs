﻿using UnityEngine;

public class ResourceManager {

    private int _gold;
    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    
    private int _souls;
    public int Souls
    {
        get { return _souls; }
        set { _souls = value; }
    }

    private float _renoun;
    public float Renoun
    {
        get { return _renoun; }
        set { _renoun = value; }
    }


    private int _cultists;
    public int Cultists
    {
        get { return _cultists; }
        set { _cultists = value; }
    }

    private int _demons;
    public int Demons
    {
        get { return _demons; }
        set { _demons = value; }
    }

    public int MaxSouls
    {
        get
        {
            MapManager map = GameObject.FindObjectOfType<MapManager>();

            return 10 + (map.prisonCount * map.prisonValue);
        }
    } 

    public ResourceManager(int gold, int souls, int renoun)
    {
        Gold = gold;
        Souls = souls;
        Renoun = renoun;
        Cultists = 2;
        Demons = 0;
    }
    
    public ResourceManager()
    {
        Gold = 0;
        Souls = 0;
        Renoun = 0;
        Cultists = 2;
        Demons = 0;
    }
    
    public bool HaveEnoughGold(int goldToSpend)
    {
        return goldToSpend <= Gold;
    }

    public bool HaveEnoughSouls(int soulsToSpend)
    {
        return soulsToSpend <= Souls;
    }

    public bool HaveEnoughRenoun(int renounToSpend)
    {
        return renounToSpend <= Renoun;
    }

    public bool SpendGold(int gold)
    {
        return SpendResources(gold);
    }

    public bool SpendSouls(int souls)
    {
        return SpendResources(0, souls);
    }

    public bool SpendRenoun(int renoun)
    {
        return SpendResources(0, 0, renoun);
    }

    public bool SpendResources(int gold = 0, int souls = 0, int renoun = 0)
    {
        if (!HaveEnoughGold(gold) || !HaveEnoughSouls(souls) || !HaveEnoughRenoun(renoun))
        {
            Debug.Log("Not enough gold / souls / renoun");
            return false;            
        }
        else
        {
            Debug.Log("Gold spend " + gold);
            Gold -= gold;
            Souls -= souls;
            Renoun -= renoun;
            return true;
        }
    }
}
