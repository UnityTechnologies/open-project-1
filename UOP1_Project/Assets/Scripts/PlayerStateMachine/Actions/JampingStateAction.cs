using UnityEngine;

namespace PlayerStateMachine.Actions
{
    public class JampingStateAction : PlayerStateAction
    {
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _cm.verticalMovement = _cm.initialJumpForce;
            
            //Apply the result and move the character in space
            _cc.Move(_cm.verticalMovement * deltaTime * Vector3.up);
        }

        public override void OnExit()
        {
            base.OnExit();
            _cm.inputJump = false;
        }
    }
}