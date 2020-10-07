﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    [CreateAssetMenu(menuName = "PluggableFSM/Character/Condition/FireButtonDown")]
    public class FireButtonDownCondition : Condition<Character>
    {
        public override bool IsMet(Character character) => character.Input.FireButtonDown;
    }
}