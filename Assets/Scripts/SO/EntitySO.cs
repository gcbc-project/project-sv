using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Scriptable Object/Entity")]
public class EntitySO : ScriptableObject
{
    public GameObject Prefab;
}
