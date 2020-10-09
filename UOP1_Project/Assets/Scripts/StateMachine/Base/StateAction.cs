using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AV.Logic
{
    public abstract class StateAction : ScriptableState
    {
        public abstract void OnUpdate();
    }
}