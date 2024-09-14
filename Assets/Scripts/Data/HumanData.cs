using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct HumanData : EntityData
{
    public GenericValue<Vector2> Location;


    public GenericValue<Vector2> GetLocation()
    {
        return Location;
    }
}
