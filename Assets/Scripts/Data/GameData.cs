using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;




[Serializable]
public class GameData
{
    public const int Game_StartGold = 1000;
    public const int Game_MaxHumans = 255;
    public const float Game_BuildingPanalty = 1.1f;
    public const int Game_ImmigrationFame = 1000;
    public const float Game_FameMultiplier = 5.0f;
    public const float Game_DayTime = 10.0f;
    public const float Game_WeekTime = Game_DayTime * 7;
    public const float Game_MonthTime = Game_WeekTime * 4;
    public const float Human_UseSqrDistance = 4.0f;
    public const float Human_MovementSpeed = 1.0f;
    public const float Human_BuffTime = 30.0f;
    public const float Building_UseTime = 3.0f;
    public const float Building_CoolTime = 5.0f;

    public ReactiveProperty<int> Gold = new();
    public ReactiveProperty<float> Time = new();
    public ReactiveProperty<int> Fame = new();
    public ReactiveProperty<int> ExpansionLevel = new();
    public ReactiveProperty<float> ExpansionDuration = new();
    public List<BuildingData> Buildings = new();
    public List<HumanData> Humans = new();

    public void Update(float timeDelta)
    {
        Time.Value += timeDelta;

        if (Time.Value % Game_MonthTime < timeDelta)
        {
            CalculateMaintainCost();
            CalculateFame();
            TryImmigration();
        }

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
        else
        {
            InitData();
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

    void InitData()
    {
        Gold.Value = Game_StartGold;
        HumanData.Create(GameManager.Get().HumanSO);
    }

    void CalculateMaintainCost()
    {
        int cost = 0;
        Dictionary<BuildingSO, float> buildingPenalty = new();

        foreach (var ele in Buildings)
        {
            BuildingSO so = ele.GetSO();
            float penalty = 1.0f;

            buildingPenalty.TryGetValue(so, out penalty);
            cost += (int)(so.BuildCost * penalty);
            buildingPenalty[so] = penalty * Game_BuildingPanalty;
        }

        Gold.Value -= cost;
    }

    void CalculateFame()
    {
        float decorateScore = 0.0f;
        foreach (var ele in Buildings)
        {
            BuildingSO so = ele.GetSO();
            if (so.Effect.Type == BuildingType.Decoration)
            {
                decorateScore += so.Effect.Value;
            }
        }

        Fame.Value += (int)(decorateScore + Game_FameMultiplier * Humans.Count);
    }

    void TryImmigration()
    {
        if (Fame.Value >= Game_ImmigrationFame && Humans.Count < Game_MaxHumans)
        {
            HumanData.Create(GameManager.Get().HumanSO);

            Fame.Value -= Game_ImmigrationFame;
        }
    }

}
