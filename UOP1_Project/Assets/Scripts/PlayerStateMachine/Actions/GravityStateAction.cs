using UnityEngine;

namespace PlayerStateMachine.Actions
{
    public class GravityStateAction : PlayerStateAction
    {
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            _cm.verticalMovement += Physics.gravity.y * _cm.gravityMultiplier * deltaTime;
            
            //Apply the result and move the character in space
            _cc.Move(_cm.verticalMovement * deltaTime * Vector3.up);
        }

        public override void OnExit()
        {
            base.OnExit();
            _cm.verticalMovement = 0;
        }
    }
}