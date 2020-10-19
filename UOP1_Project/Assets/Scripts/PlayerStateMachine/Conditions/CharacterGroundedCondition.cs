using StateMachines.Mono;
using UnityEngine;

namespace PlayerStateMachine.Conditions
{
    public class CharacterGroundedCondition : MonoCondition
    {
        private CharacterMotor _characterMotor;

        private void Awake()
        {
            _characterMotor = GetComponentInParent<CharacterMotor>();
        }

        protected override bool Evaluate() => _characterMotor.IsGrounded;
    }
}