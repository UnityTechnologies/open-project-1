using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+ "SubscribeToInputEvents",fileName = "SubscribeToInputEvents")]
    public class SubscribeToInputEvents : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            InputHandler handlerInput = _controller.HandlerInput;

            handlerInput.SubscribeToEvents();
        }
    }

}
