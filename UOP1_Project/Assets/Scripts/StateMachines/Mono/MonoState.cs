using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Mono
{
    public class MonoState : MonoBehaviour, IState
    {
        private List<IStateAction> _actions;
        private List<ITransition> _transitions;
        
        protected virtual void Awake()
        {
            Debug.Log("MonoState.Awake()");
            _transitions = new List<ITransition>();
            GetComponentsInChildren<ITransition>(_transitions);
            _actions = new List<IStateAction>();
            GetComponentsInChildren<IStateAction>(_actions);
        }

        public virtual void OnEnter()
        {
            foreach (var action in _actions)
                action.OnEnter();
        }

        public virtual void OnUpdate(float deltaTime)
        {
            foreach (var action in _actions)
                action.OnUpdate(deltaTime);
        }

        public virtual void OnExit()
        {
            foreach (var action in _actions)
                action.OnExit();
        }
    }
}