using System;
using UnityEngine;


public enum BuildingType
{
    None,
    House,
    //Road,
    Shop,
    Decoration
}

[Serializable]
public struct BuildingEffect
{
    public BuildingType Type;
    public int Value;
}

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Scriptable Object/Building")]
public class BuildingSO : ScriptableObject
{
    public int BuildCost;
    public int MaintainCost;
    public int RequiredFame;
    public float BuildTime;
    public BuildingEffect[] BuildingEffects;
}