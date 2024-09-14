using System;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    public float Value;
}

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Scriptable Object/Building")]
public class BuildingSO : EntitySO
{
    public string Name;
    public int BuildCost;
    public int MaintainCost;
    public int RequiredFame;
    public float BuildTime;
    public TileBase Tile;
    public Vector2Int Size;
    public BuildingEffect Effect;
}