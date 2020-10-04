using UnityEngine;
using System.Collections;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+ "CalculateGravityContribution", fileName = "CalculateGravityContribution")]
    public class CalculateGravityContribution : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            MovementHandler handlerMove = _controller.HandlerMovement;
            InputHandler handlerInput = _controller.HandlerInput;

           
        }
       
    }

}
