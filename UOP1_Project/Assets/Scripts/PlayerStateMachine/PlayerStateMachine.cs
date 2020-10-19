using StateMachines.Mono;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerStateMachine : MonoStateMachine
    {
        [SerializeField] private PlayerState _debugCurrentState = default;
        

        private void Start()
        {
            PlayerState[] states = GetComponentsInChildren<PlayerState>();
            foreach (var state in states)
            {
                state.Initialize(this);
            }

            _stateMachine.StateChanged += state => _debugCurrentState = (PlayerState) state;
        }
    }
}