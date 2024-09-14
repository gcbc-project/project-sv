using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BuildingData : EntityData
{
    public GenericValue<Vector2> Location;
    public GenericValue<BuildingSO> SO;

    public GenericValue<Vector2> GetLocation()
    {
        return Location;
    }
}
