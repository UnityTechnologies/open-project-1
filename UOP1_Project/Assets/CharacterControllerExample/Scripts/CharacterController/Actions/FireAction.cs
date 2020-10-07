using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Action/Fire")]
    public class FireAction : Action<Character>
    {
        public override void Act(Character character) => character.CurrentGun.Fire();
    }
}