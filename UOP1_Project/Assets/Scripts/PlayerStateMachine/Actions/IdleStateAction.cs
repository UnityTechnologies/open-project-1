using UnityEngine;

namespace PlayerStateMachine.Actions
{
    public class IdleStateAction : PlayerStateAction
    {
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _cc.Move(Vector3.down * deltaTime);
        }
    }
}