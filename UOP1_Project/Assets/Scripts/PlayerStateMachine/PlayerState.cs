using StateMachines.Mono;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerState : MonoState
    {
        public virtual void Initialize(PlayerStateMachine context)
        {
            gameObject.SetActive(context.CurrentState == this);
        }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {name}");
            base.OnEnter();
            gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            Debug.Log($"Exit: {name}");
            base.OnExit();
            gameObject.SetActive(false);
        }
    }
}