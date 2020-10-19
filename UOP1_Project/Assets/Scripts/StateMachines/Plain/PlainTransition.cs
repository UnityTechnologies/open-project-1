using System;

namespace StateMachines.Plain
{
    public class PlainTransition : ITransition
    {
        private readonly IState _toState;
        private readonly Func<bool> _func;

        public PlainTransition(IState toState, Func<bool> func)
        {
            _toState = toState;
            _func = func;
        }

        public IState TargetState => _toState;

        public virtual bool Evaluate()
        {
            return _func.Invoke();
        }

        public void OnEnter() { }

        public void OnUpdate(float deltaTime) { }

        public void OnExit() { }
    }
}