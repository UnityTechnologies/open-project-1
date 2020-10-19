using StateMachines;
using StateMachines.Mono;

namespace PlayerStateMachine
{
    public class PlayerTransition : MonoTransition
    {
        private ICondition[] _conditions;

        private void Awake()
        {
            _conditions = GetComponentsInChildren<ICondition>();
        }

        public override bool Evaluate()
        {
            foreach (var condition in _conditions)
            {
                if (!condition.Value)
                    return false;
            }
            return true;
        }

        private void OnValidate()
        {
            name = $"To_{_targetState.name}_Transition";
        }
    }
}