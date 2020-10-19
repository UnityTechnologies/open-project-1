namespace StateMachines
{
    public interface ITransition : IStateMachineEntity
    {
        IState TargetState { get; }
        
        bool Evaluate();
    }
}