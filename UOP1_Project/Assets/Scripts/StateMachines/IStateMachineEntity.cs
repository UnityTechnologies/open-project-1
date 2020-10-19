namespace StateMachines
{
    public interface IStateMachineEntity
    {
        void OnEnter();
        
        // Personally, I would like to name this methods without 'On' prefix,
        // but Unity doesn't allow to use method with name 'Update' that has arguments. 
        void OnUpdate(float deltaTime);
        
        void OnExit();
    }
}