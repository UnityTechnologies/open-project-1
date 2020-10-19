namespace StateMachines.Plain.Builder
{
    public class Example
    {
        public void Execute()
        {
            // applies input to character
            var moveAction = new PlainStateAction(null, null, null);
            // applies input to character when it is in the air (falling or jumping)
            var inAirMoveAction = new PlainStateAction(null, null, null);
            // apples one time jump impulse to character
            var jumpAction = new PlainStateAction(null, null, null);
            // applies gravity force to character
            var gravityAction = new PlainStateAction(null, null, null);
            
            var idleState = new PlainState(new IStateAction[]{ /* in idle do nothing */});
            var runState = new PlainState(new []{moveAction, gravityAction});
            var jumpState = new PlainState(new []{inAirMoveAction, jumpAction});
            var fallState = new PlainState(new []{inAirMoveAction, gravityAction});
            float speed = 10;
            bool jump = true;
            bool isGrounded = false;
            IStateMachine stateMachine = new PlainStateMachineBuilder()
                .Default(idleState)
                
                .From(idleState)
                .To(runState, () => speed >= 10)
                .To(jumpState, () => jump)
                
                .From(runState)
                .To(idleState, () => speed <= 10)
                .To(jumpState, () => jump)
                .To(fallState, () => !isGrounded)
                
                .From(jumpState)
                .To(fallState, () => true)
                
                .From(fallState)
                .To(idleState, () => isGrounded)
                .Build();
        }
    }
}