using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GenericValue<T>
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
                NewDelegate?.Invoke(value);
                BothDelegate?.Invoke(value, oldValue);
            }
        }
    }

    public UnityEvent<T> NewDelegate;
    public UnityEvent<T, T> BothDelegate;
}


[Serializable]
public struct GameData
{
    public GenericValue<ulong> Gold;
    public GenericValue<ulong> Time;
    public GenericValue<ulong> Fame;
    public GenericValue<uint> ExpansionLevel;
    public GenericValue<ulong> ExpansionDuration;
    public GenericValue<BuildingData>[] Builldings;
}
