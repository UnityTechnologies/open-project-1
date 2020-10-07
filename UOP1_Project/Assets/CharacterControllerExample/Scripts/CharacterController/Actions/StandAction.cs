using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Action/Stand")]
    public class StandAction : Action<Character>
    {
        public override void Act(Character character) => character.Stand();
    }
}