using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class EntityData
{
    protected EntityData()
    {
        Location.Subscribe(OnLocationChanged);
    }

    public ReactiveProperty<Vector2> Location = new();

    [SerializeField]
    protected EntitySO _so;

    [NonSerialized]
    public GameObject gameObject;


    void OnLocationChanged(Vector2 vector)
    {
        if (gameObject)
        {
            gameObject.transform.position = vector;
        }
    }

    public virtual void Load()
    {
        if (gameObject == null)
        {
            gameObject = GameObject.Instantiate(_so.Prefab);
        }
        OnLocationChanged(Location.Value);
    }
}
