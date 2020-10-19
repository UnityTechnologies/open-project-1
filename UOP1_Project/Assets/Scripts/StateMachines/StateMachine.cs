using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StateMachines
{
    public class StateMachine : IStateMachine
    {
        public IState CurrentState => _currentState;

        private IState _currentState;
        private readonly IDictionary<IState, IEnumerable<ITransition>> _transitionsMap;

        public event Action<IState> StateChanged; 
        
        public StateMachine(IState state, IDictionary<IState, IEnumerable<ITransition>> transitionsMap)
        {
            _currentState = state;
            _transitionsMap = transitionsMap;
            _currentState.OnEnter();
        }

        public void ChangeState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
            OnStateChanged();
        }

        public void OnUpdate(float deltaTime)
        {
            IEnumerable<ITransition> transitions = _transitionsMap[CurrentState];
            IState nextState = null;
            foreach (var transition in transitions)
            {
                if (transition.Evaluate())
                {
                    nextState = transition.TargetState;
                    Debug.Log($"Transiting to {nextState} because of {transition}", transition as Object);
                    break;
                }
            }
            if (nextState != null)
                ChangeState(nextState);
            
            _currentState?.OnUpdate(deltaTime);
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(CurrentState);
        }
    }
}