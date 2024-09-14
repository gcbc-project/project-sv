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
    protected EntitySO SO;

    [NonSerialized]
    public GameObject Entity;


    void OnLocationChanged(Vector2 vector)
    {
        if (Entity)
        {
            Entity.gameObject.transform.position = vector;
        }
    }

    public virtual void Load()
    {
        if (Entity == null)
        {
            Entity = GameObject.Instantiate(SO.Prefab);
        }
        OnLocationChanged(Location.Value);
    }
}
