using System;
using System.Collections;
using System.Collections.Generic;
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

    public ObservableValue<HumanOutfit> Outfit;
    public ObservableValue<BuildingSO>[] PrevBuildings = new ObservableValue<BuildingSO>[3];

    [NonSerialized]
    public ObservableValue<HumanBuff> Buff;
    [NonSerialized]
    public ObservableValue<BuildingData> NextBuilding;

    public void Update(float timeDelta)
    {
        Buff.Value.Update(timeDelta);
    }
}
