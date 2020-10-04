using UnityEngine;
using System.Collections;
using UnityEngine.XR;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatActionRoot+ "CalculateInputVectorBasedOnGameCamera",fileName = "CalculateInputVectorBasedOnGameCamera")]
    public class CalculateInputVectorBasedOnGameCamera : CombatAction
    {
        public override void Act(CombatStateMachineController _controller)
        {
            InputHandler handlerInput = _controller.HandlerInput;
            MovementHandler handlerMove = _controller.HandlerMovement;

            Transform _gameplayCam = _controller.HandlerMovement.GameplayCameraTransform;


            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = _gameplayCam.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _gameplayCam.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = cameraRight.normalized * handlerInput.RawMovementInput.x +
                cameraForward.normalized * handlerInput.RawMovementInput.y;

            handlerMove.inputVector = Vector3.ClampMagnitude(adjustedMovement, 1f);
        }
    }

}
