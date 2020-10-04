using UnityEngine;
using System.Collections;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+"ApplyTurning",fileName ="ApplyTurning")]
    public class ApplyTurning : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            MovementHandler moveHandler = _controller.HandlerMovement;

            if (moveHandler.movementVector.sqrMagnitude >= .02f)
            {
                float targetRotation = Mathf.Atan2(moveHandler.movementVector.x, moveHandler.movementVector.z) * Mathf.Rad2Deg;
                moveHandler.TransformComponent.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                    moveHandler.TransformComponent.eulerAngles.y,
                    targetRotation,
                    ref moveHandler.turnSmoothSpeed,
                    moveHandler.HandlerData.TurnSmoothTime);
            }
        }
    }

}
