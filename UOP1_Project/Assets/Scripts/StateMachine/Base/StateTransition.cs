
using System;
using UnityEngine;

namespace AV.Logic
{
    [Serializable]
    public class StateTransition
    {
        // Used to avoid type checks
        internal enum StateType
        {
            Node,
            Action,
            Decision
        }
        
        [Serializable]
        public struct TransitionAction
        {
            public TransitionTrigger trigger;
            public ScriptableState state;
#pragma warning disable 649
            [SerializeField] 
            internal StateType type; // assigned by StateNodeEditor
#pragma warning restore 649
        }
        
        public bool enabled;
        public StateDecision[] decisions; // TODO: Add custom condition (Or, When etc..)
        public TransitionAction[] actions;
    }
}