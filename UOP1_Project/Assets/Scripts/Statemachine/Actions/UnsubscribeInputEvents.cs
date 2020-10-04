
using UnityEngine;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName = CSMUtility.CombatActionRoot + "UnsubscribeInputEvents", fileName = "UnsubscribeInputEvents")]
    public class UnsubscribeInputEvents : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            InputHandler handlerInput = _controller.HandlerInput;

            handlerInput.UnsubscribeEvents();
        }
    }

}
