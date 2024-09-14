using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct ObservableValue<T>
{
    [SerializeField]
    T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                T oldValue = _value;
                _value = value;
                OnChanged?.Invoke(value, oldValue);
            }
        }
    }

    public UnityEvent<T, T> OnChanged;
}


[Serializable]
public class GameData
{
    GameData()
    {
        Buildings.Value = new();
        Humans.Value = new();
    }

    public ObservableValue<uint> Gold;
    public ObservableValue<float> Time;
    public ObservableValue<uint> Fame;
    public ObservableValue<uint> ExpansionLevel;
    public ObservableValue<float> ExpansionDuration;
    public ObservableValue<List<BuildingData>> Buildings;
    public ObservableValue<List<HumanData>> Humans;

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
}
