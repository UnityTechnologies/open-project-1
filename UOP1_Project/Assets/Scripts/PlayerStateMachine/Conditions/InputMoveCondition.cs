using StateMachines.Mono;

namespace PlayerStateMachine.Conditions
{
    public class InputMoveCondition : MonoCondition
    {
        private CharacterMotor _characterMotor;

        protected override bool Evaluate() => _characterMotor.inputVector.magnitude > 0.01f;

        private void Awake()
        {
            _characterMotor = GetComponentInParent<CharacterMotor>();
        }
    }
}