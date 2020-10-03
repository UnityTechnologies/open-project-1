using UnityEngine;

namespace AV.Logic
{
    public abstract class StateDecision : ScriptableState
    {
        internal override void BeginUpdate(StateMachine machine)
        {
            this.machine = machine;
            OnDecide();
        }
        
        public abstract bool OnDecide();
        public virtual void OnTrue(){}
        public virtual void OnFalse(){}
    }
}