using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class GameData
{
    public ReactiveProperty<uint> Gold = new();
    public ReactiveProperty<float> Time = new();
    public ReactiveProperty<uint> Fame = new();
    public ReactiveProperty<uint> ExpansionLevel = new();
    public ReactiveProperty<float> ExpansionDuration = new();
    public ReactiveProperty<List<BuildingData>> Buildings = new();
    public ReactiveProperty<List<HumanData>> Humans = new();

    public void Update(float timeDelta)
    {
        Time.Value += timeDelta;

        if (ExpansionDuration.Value > 0.0f)
        {
            ExpansionDuration.Value -= timeDelta;
            if (ExpansionDuration.Value <= 0.0f)
            {
                ExpansionLevel.Value += 1;
            }
        }

        foreach (var ele in Buildings.Value)
        {
            ele.Update(timeDelta);
        }

        foreach (var ele in Humans.Value)
        {
            ele.Update(timeDelta);
        }
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("save"), this);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("save", JsonUtility.ToJson(this));
    }
}
