using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AV.Logic
{
    public abstract class StateAction : ScriptableState
    {
        internal override void BeginUpdate(StateMachine machine)
        {
            this.machine = machine;
            OnUpdate();
        }
        
        protected abstract void OnUpdate();
    }
}