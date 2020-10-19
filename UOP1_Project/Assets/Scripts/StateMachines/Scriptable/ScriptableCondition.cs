using UnityEngine;

namespace StateMachines.Scriptable
{
    public abstract class ScriptableCondition : ScriptableObject, ICondition
    {
        public abstract bool Value { get; }
        
        public virtual void OnEnter() { }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnExit() { }
    }
}