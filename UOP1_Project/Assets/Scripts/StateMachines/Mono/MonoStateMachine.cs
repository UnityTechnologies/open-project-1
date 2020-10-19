using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Mono
{
    public class MonoStateMachine : MonoBehaviour, IStateMachine
    {
        [SerializeField] private MonoState _defaultState = default;
        protected IStateMachine _stateMachine;

        public IStateMachine StateMachine => _stateMachine ?? (_stateMachine = new StateMachine(_defaultState, GetTransitionTable()));

        protected virtual void Awake()
        {
        }

        public IState CurrentState => _stateMachine.CurrentState;
        
        public event Action<IState> StateChanged
        {
            add => StateMachine.StateChanged += value;
            remove => StateMachine.StateChanged -= value;
        }

        public void ChangeState(IState state)
        {
            StateMachine.ChangeState(state);
        }

        public void OnUpdate(float deltaTime)
        {
            StateMachine.OnUpdate(deltaTime);
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        private IDictionary<IState, IEnumerable<ITransition>> GetTransitionTable()
        {
            List<MonoState> states = new List<MonoState>();
            GetComponentsInChildren(states);
            
            IDictionary<IState, IEnumerable<ITransition>> table = new Dictionary<IState, IEnumerable<ITransition>>();
            foreach (var state in states)
            {
                table.Add(state, state.GetComponentsInChildren<ITransition>());
            }

            return table;
        }
    }
}