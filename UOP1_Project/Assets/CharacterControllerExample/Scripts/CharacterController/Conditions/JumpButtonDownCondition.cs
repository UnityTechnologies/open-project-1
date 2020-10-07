using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Condition/JumpButtonDown")]
    public class JumpButtonDownCondition : Condition<Character>
    {
        public override bool IsMet(Character character) => character.Input.JumpButtonDown;
    }
}