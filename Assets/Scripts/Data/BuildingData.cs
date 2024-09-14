using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class BuildingSlot
{
    public ReactiveProperty<HumanData> UsingHuman = new();
    public ReactiveProperty<float> RemainTime = new();
    public ReactiveProperty<float> CoolTime = new();


    public bool SetHuman(HumanData humanData)
    {
        if (UsingHuman.Value == null && CoolTime.Value <= 0.0f)
        {
            UsingHuman.Value = humanData;
            RemainTime.Value = 3.0f;
            CoolTime.Value = 5.0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update(float timeDelta)
    {
        if (RemainTime.Value > 0.0f)
        {
            RemainTime.Value -= timeDelta;
        }
        else
        {
            UsingHuman.Value = null;
            if (CoolTime.Value > 0.0f)
            {
                CoolTime.Value -= timeDelta;
            }
        }
    }
}


[Serializable]
public class BuildingData : EntityData
{
    private BuildingData() {}
    public static BuildingData Create(BuildingSO buildingSO)
    {
        var instance = new BuildingData();
        instance.SO = buildingSO;
        instance.Load();
        return instance;
    }

    public ReactiveProperty<float> BuildingTime = new();

    ReactiveProperty<BuildingSlot>[] Slots = { new(new()), new(new()), new(new()) };

    public void Update(float timeDelta)
    {
        if (BuildingTime.Value > 0.0f)
        {
            BuildingTime.Value -= timeDelta;
        }

        foreach (var ele in Slots)
        {
            ele.Value.Update(timeDelta);
        }
    }

    public bool UseBuilding(HumanData humanData)
    {
        foreach (var ele in Slots)
        {
            if (ele.Value.SetHuman(humanData))
            {
                return true;
            }
        }
        return false;
    }
}
