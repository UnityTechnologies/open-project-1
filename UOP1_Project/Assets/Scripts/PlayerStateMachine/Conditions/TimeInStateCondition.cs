using StateMachines;
using StateMachines.Mono;
using UnityEngine;

namespace PlayerStateMachine.Conditions
{
    public class TimeInStateCondition : MonoCondition
    {
        [SerializeField] private MonoState _state = default;
        [SerializeField] private float _time = 1f;

        private float _entryTime;

        private IStateMachine _stateMachine;
        
        private void Start()
        {
            _stateMachine = GetComponentInParent<IStateMachine>();
            _stateMachine.StateChanged += OnStateChanged;
        }

        private void OnDestroy()
        {
            _stateMachine.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(IState state)
        {
            if ((MonoState) state == _state)
            {
                _entryTime = Time.time;
            }
        }

        protected override bool Evaluate()
        {
            return Time.time > _entryTime + _time;
        }
    }
}