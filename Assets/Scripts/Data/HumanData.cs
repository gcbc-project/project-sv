using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct HumanOutfit
{
    uint Head;
    uint Body;
}

public struct HumanBuff
{
    float _multiplier;
    public float Multiplier { get => _multiplier; }
    float _remainTime;

    public void Update(float timeDelta)
    {
        if (_remainTime > 0.0f)
        {
            _remainTime -= timeDelta;
        }
        else
        {
            _multiplier = 1.0f;
        }
    }

    public void SetBuff(float multiplier)
    {
        _multiplier = multiplier;
        _remainTime = GameData.Human_BuffTime;
    }
}

public enum HumanState
{
    None,
    Move,
    Wait,
    Use,
}

[Serializable]
public class HumanData : EntityData
{
    HumanData() {}

    public static HumanData Create(EntitySO entitySO)
    {
        var instance = new HumanData();
        instance._so = entitySO;
        instance.Load();

        GameManager.Get().Data.Humans.Add(instance);
        return instance;
    }


    public ReactiveProperty<HumanOutfit> Outfit = new();
    [SerializeField]
    List<BuildingSO> _prevBuildings = new();

    [NonSerialized]
    HumanBuff _buff;
    [NonSerialized]
    BuildingData _nextBuilding = null;

    HumanState _state;

    public void Update(float timeDelta)
    {
        _buff.Update(timeDelta);

        switch (_state)
        {
            case HumanState.Move:
                MoveToNextBuilding(timeDelta);
                if (IsCloseToNextBuilding())
                {
                    SetState(HumanState.Wait);
                }
                break;
            case HumanState.Wait:
                TryUseBuilding();
                break;
            case HumanState.Use:
                // do nothing
                break;
        }
    }

    public override void Load()
    {
        base.Load();

        SetState(HumanState.Move);
    }

    public void Buff(float multiplier)
    {
        _buff.SetBuff(multiplier);
    }
    
    public void Earn(float gold)
    {
        GameManager.Get().Data.Gold.Value += (int)(_buff.Multiplier * gold);
    }

    public void CompleteUsing(BuildingSO buildingSO)
    {
        _prevBuildings.Add(buildingSO);
        if (_prevBuildings.Count > 3)
        {
            _prevBuildings.RemoveAt(0);
        }
        SpeechBubble.GetRandomDialog(buildingSO.Name, gameObject.transform);
        SetState(HumanState.Move);
    }

    void SetState(HumanState humanState)
    {
        switch (humanState)
        {
            case HumanState.Move:
                CalculateNextBuilding();
                break;
            case HumanState.Wait:
            case HumanState.Use:
                break;
        }
        _state = humanState;
    }

    void CalculateNextBuilding()
    {
        List<BuildingSO> buildingSOs = new();
        foreach (var ele in GameManager.Get().Data.Buildings)
        {
            if (_prevBuildings.Contains(ele.GetSO()) == false)
            {
                buildingSOs.Add(ele.GetSO());
            }
        }

        if (buildingSOs.Count == 0)
        {
            foreach (var ele in _prevBuildings)
            {
                if (ele != null)
                {
                    buildingSOs.Add(ele);
                    break;
                }
            }
        }

        var index = Random.Range(0, buildingSOs.Count);
        _nextBuilding = GameManager.Get().Data.GetNearestBuilding(Location.Value, buildingSOs.ToArray()[index]);

        if (_nextBuilding == null)
        {
            Debug.LogError("no next building");
        }
    }

    void MoveToNextBuilding(float timeDelta)
    {
        Location.Value += (_nextBuilding.Location.Value - Location.Value).normalized * timeDelta * _buff.Multiplier * GameData.Human_MovementSpeed;
    }

    bool IsCloseToNextBuilding()
    {
        return (_nextBuilding.Location.Value - Location.Value).sqrMagnitude < GameData.Human_UseSqrDistance;
    }

    void TryUseBuilding()
    {
        if (_nextBuilding == null)
        {
            Debug.LogError("no building to wait");
        }

        if (_nextBuilding.UseBuilding(this))
        {
            SetState(HumanState.Use);
        }
    }
}
