using StateMachines.Mono;

namespace PlayerStateMachine.Conditions
{
    public class InputJumpCondition : MonoCondition
    {
        private CharacterMotor _characterMotor;

        private void Awake()
        {
            _characterMotor = GetComponentInParent<CharacterMotor>();
        }
        
        protected override bool Evaluate() => _characterMotor.inputJump;

    }
}