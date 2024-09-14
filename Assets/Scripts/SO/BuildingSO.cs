using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Scriptable Object/Building")]
public class BuildingSO : ScriptableObject
{
    public int BuildCost;
    public int MaintainCost;
    public int RequiredFame;
    public float BuildTime;
}