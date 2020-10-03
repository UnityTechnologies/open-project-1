using System.Collections;
using System.Collections.Generic;
using AV.Logic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Data")]
public class NpcData : ScriptableObject
{
    [Range(0, 100)]
    public int energy;
    public float sightRadius;
}
//
/// <summary>
/// Non-shared NPC data.
/// </summary>
public struct NpcState : IStateData
{
    public int energy;

    public NpcState(NpcData data)
    {
        energy = data.energy;
    }
}