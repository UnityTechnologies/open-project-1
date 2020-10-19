using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Scriptable
{
    public abstract class ScriptableState : ScriptableObject, IState
    {
        [SerializeField] private List<ScriptableStateAction> _actions;

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