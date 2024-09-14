using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


[Serializable]
public struct HumanOutfit
{
    uint Head;
    uint Body;
}

public struct HumanBuff
{
    float Multiplier;
    float RemainTime;

    public void Update(float timeDelta)
    {
        if (RemainTime > 0.0f)
        {
            RemainTime -= timeDelta;
        }
        else
        {
            Multiplier = 1.0f;
        }
    }
}

[Serializable]
public class HumanData : EntityData
{
    HumanData()
    {
        PrevBuildings = new ReactiveProperty<BuildingSO>[3];
        foreach (var ele in PrevBuildings)
        {
            ele.Value = new();
        }
    }

    public static HumanData Create(EntitySO entitySO)
    {
        var instance = new HumanData();
        instance.SO = entitySO;
        instance.Load();
        return instance;
    }


    public ReactiveProperty<HumanOutfit> Outfit = new();
    public ReactiveProperty<BuildingSO>[] PrevBuildings;

    [NonSerialized]
    public ReactiveProperty<HumanBuff> Buff = new();
    [NonSerialized]
    public ReactiveProperty<BuildingData> NextBuilding = new();

    public void Update(float timeDelta)
    {
        Buff.Value.Update(timeDelta);
    }
}
