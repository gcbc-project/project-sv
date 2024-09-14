using System;
using UniRx;

public class BuildingSlot
{
    HumanData _usingHuman;
    float _remainTime;
    float _coolTime;
    public Action<HumanData> OnComplete;


    public bool SetHuman(HumanData humanData)
    {
        if (_usingHuman == null && _coolTime <= 0.0f)
        {
            _usingHuman = humanData;
            _remainTime = GameData.Building_UseTime;
            _coolTime = GameData.Building_CoolTime;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update(float timeDelta)
    {
        if (_remainTime > 0.0f)
        {
            _remainTime -= timeDelta;
            if (_remainTime <= 0.0f)
            {
                OnComplete?.Invoke(_usingHuman);
                _usingHuman = null;
            }
        }
        else if (_coolTime > 0.0f)
        {
            _coolTime -= timeDelta;
        }
    }
}


[Serializable]
public class BuildingData : EntityData
{
    private BuildingData() 
    {
        foreach (var ele in Slots)
        {
            ele.OnComplete = OnComplete;
        }
    }
    public static BuildingData Create(BuildingSO buildingSO)
    {
        var instance = new BuildingData();
        instance._so = buildingSO;
        instance.Load();

        GameManager.Get().Data.Buildings.Add(instance);
        return instance;
    }

    public ReactiveProperty<float> BuildingTime = new();

    BuildingSlot[] Slots = { new(), new(), new() };

    public void Update(float timeDelta)
    {
        if (BuildingTime.Value > 0.0f)
        {
            BuildingTime.Value -= timeDelta;
        }

        foreach (var ele in Slots)
        {
            ele.Update(timeDelta);
        }
    }

    public BuildingSO GetSO()
    {
        return (BuildingSO)_so;
    }

    public bool UseBuilding(HumanData humanData)
    {
        foreach (var ele in Slots)
        {
            if (ele.SetHuman(humanData))
            {
                return true;
            }
        }
        return false;
    }

    void OnComplete(HumanData humanData)
    {
        var so = GetSO();
        var buildingEffect = so.Effect;

        switch (buildingEffect.Type)
        {
            case BuildingType.House:
                humanData.Buff(buildingEffect.Value);
                break;
            case BuildingType.Shop:
                humanData.Earn(buildingEffect.Value);
                break;
        }

        humanData.CompleteUsing(so);
    }
}
