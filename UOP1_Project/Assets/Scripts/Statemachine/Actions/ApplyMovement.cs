using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+"ApplyMovement",fileName ="ApplyMovement")]
    public class ApplyMovement : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            MovementHandler handlerMove = _controller.HandlerMovement;

            handlerMove.CharControll.Move(handlerMove.movementVector * Time.deltaTime);

        }

    }

}
