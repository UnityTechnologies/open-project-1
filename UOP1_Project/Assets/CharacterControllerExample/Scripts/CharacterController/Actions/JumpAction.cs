using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Action/Jump")]
    public class JumpAction : Action<Character>
    {
        public override void Act(Character character) => character.Jump();
    }
}