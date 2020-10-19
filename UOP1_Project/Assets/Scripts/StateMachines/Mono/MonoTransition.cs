using UnityEngine;

namespace StateMachines.Mono
{
    public abstract class MonoTransition : MonoBehaviour, ITransition
    {
        [SerializeField] protected MonoState _targetState = default;

        public virtual IState TargetState => _targetState;
        
        public abstract bool Evaluate();
        
        public virtual void OnEnter() { }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnExit() { }
    }
}