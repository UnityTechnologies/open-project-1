using System;
using System.Collections.Generic;

namespace StateMachines.Plain
{
    public class PlainState : IState
    {
        private readonly IEnumerable<IStateAction> _actions;

        public PlainState(IEnumerable<IStateAction> actions)
        {
            _actions = actions;
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