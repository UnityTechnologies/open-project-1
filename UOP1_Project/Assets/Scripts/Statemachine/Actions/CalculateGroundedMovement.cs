using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+ "CalculateGroundedMovement", fileName = "CalculateGroundedMovement")]
    public class CalculateGroundedMovement : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            MovementHandler handlerMove = _controller.HandlerMovement;

            handlerMove.movementVector = handlerMove.inputVector * handlerMove.HandlerData.Speed;

            if(!_controller.HandlerInput.HasJumpInput)
            {
                handlerMove.verticalMovement = -5f;
                handlerMove.gravityContributionMultiplier = 0f;
            }

            handlerMove.movementVector = new Vector3
                (
                    handlerMove.movementVector.x,
                    handlerMove.verticalMovement,
                    handlerMove.movementVector.z
                );
            
        }
    }

}
