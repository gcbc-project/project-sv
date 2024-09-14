using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;




[Serializable]
public class GameData
{
    public const float Human_BuffTime = 30.0f;
    public const float Building_UseTime = 3.0f;
    public const float Building_CoolTime = 5.0f;

    public ReactiveProperty<uint> Gold = new();
    public ReactiveProperty<float> Time = new();
    public ReactiveProperty<uint> Fame = new();
    public ReactiveProperty<uint> ExpansionLevel = new();
    public ReactiveProperty<float> ExpansionDuration = new();
    public List<BuildingData> Buildings = new();
    public List<HumanData> Humans = new();

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

        foreach (var ele in Buildings)
        {
            ele.Update(timeDelta);
        }

        foreach (var ele in Humans)
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

    public BuildingData GetNearestBuilding(Vector2 vector2, BuildingSO buildingSO)
    {
        BuildingData buildingData = null;
        float distance = float.MaxValue;
        foreach (var ele in Buildings)
        {
            if (ele.GetSO() != buildingSO)
            {
                continue;
            }

            float newDistance = (vector2 - ele.Location.Value).sqrMagnitude;
            if (distance > newDistance)
            {
                distance = newDistance;
                buildingData = ele;
            }
        }

        return buildingData;
    }
}
