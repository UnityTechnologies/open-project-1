using System;

namespace StateMachines.Plain
{
    public class PlainStateAction : IStateAction
    {
        private readonly Action _enter;
        private readonly Action<float> _update;
        private readonly Action _exit;

        public PlainStateAction(Action enter, Action<float> update, Action exit)
        {
            _enter = enter;
            _update = update;
            _exit = exit;
        }

        public void OnEnter()
        {
            _enter?.Invoke();
        }

        public void OnUpdate(float deltaTime)
        {
            _update?.Invoke(deltaTime);
        }

        public void OnExit()
        {
            _exit?.Invoke();
        }
    }
}