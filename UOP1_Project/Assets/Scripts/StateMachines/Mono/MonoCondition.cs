using UnityEngine;

namespace StateMachines.Mono
{
    public abstract class MonoCondition : MonoBehaviour, ICondition
    {
        [SerializeField] private bool _inverse = false;

        public bool Value => _inverse ? !Evaluate() : Evaluate();

        public virtual void OnEnter() { }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnExit() { }
        
        protected abstract bool Evaluate();
        
        private void OnValidate()
        {
            name = GetType().Name;
            if (_inverse)
                name = "Not_" + name;
        }
    }
}