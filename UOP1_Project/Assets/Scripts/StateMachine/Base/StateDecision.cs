using UnityEngine;

namespace AV.Logic
{
    public abstract class StateDecision : ScriptableState
    {
        public abstract bool OnDecide();
        public virtual void AfterDecision(bool decided){}
    }
}