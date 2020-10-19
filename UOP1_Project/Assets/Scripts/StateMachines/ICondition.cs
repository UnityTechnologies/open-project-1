namespace StateMachines
{
    public interface ICondition : IStateMachineEntity
    {
        bool Value { get; }
    }
}