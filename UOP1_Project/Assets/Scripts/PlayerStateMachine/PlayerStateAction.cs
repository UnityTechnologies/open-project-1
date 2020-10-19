using StateMachines.Mono;
using UnityEngine;

namespace PlayerStateMachine
{
    public abstract class PlayerStateAction : MonoStateAction
    {
        protected CharacterController _cc;
        protected CharacterMotor _cm;

        public void Awake()
        {
            _cm = GetComponentInParent<CharacterMotor>();
            _cc = GetComponentInParent<CharacterController>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            Debug.Log($"Updating {name}.{GetType().Name}");
        }
    }
}