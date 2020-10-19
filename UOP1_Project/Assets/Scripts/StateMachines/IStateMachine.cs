using System;

namespace StateMachines
{
    public interface IStateMachine
    {
        IState CurrentState { get; }

        event Action<IState> StateChanged; 
        
        void ChangeState(IState state);

        void OnUpdate(float deltaTime);
    }
}