using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Condition/IsGrounded")]
    public class IsGroundedCondition : Condition<Character>
    {
        public override bool IsMet(Character character) => character.IsGrounded;
    }
}