using System.Collections;
using System.Collections.Generic;
using AV.Logic;
using UnityEngine;

namespace Island.AI.States
{
    [CreateAssetMenu(menuName = "NPC/Decisions/Hunger")]
    public class HungerDecision : StateDecision
    {
        public override bool OnDecide()
        {
            var state = GetData<NpcState>();
            return state.energy < 50;
        }
    }
}