using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Condition/DuckButtonDown")]
    public class DuckButtonDownCondition : Condition<Character>
    {
        public override bool IsMet(Character character) => character.Input.DuckButtonDown;
    }
}