using System;
using UnityEngine;

namespace StateMachines.Scriptable
{
    public abstract class ScriptableStateMachine : ScriptableObject, IStateMachine
    {
        [SerializeField] private ScriptableState _defaultState = default;
        [SerializeField] private ScriptableTransitionTable _scriptableTransitionTable = default;

        protected IStateMachine _stateMachine;

        protected virtual void Awake()
        {
            _stateMachine = new StateMachine(_defaultState, _scriptableTransitionTable.Get());
        }

        public virtual IState CurrentState => _stateMachine.CurrentState;

        public event Action<IState> StateChanged;

        public virtual void ChangeState(IState state)
        {
            _stateMachine.ChangeState(state);
        }

        public virtual void OnUpdate(float deltaTime)
        {
            _stateMachine.OnUpdate(deltaTime);
        }
    }
}