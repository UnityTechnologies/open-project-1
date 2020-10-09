
using System;
using UnityEngine;

namespace AV.Logic
{
    [Serializable]
    public class StateTransition
    {
        internal enum ActionType
        {
            Action,
            ChangeState
        }
        
        [Serializable]
        public struct Decision
        {
            public DecisionCondition condition;
            public StateDecision state;
        }
        
        [Serializable]
        public struct DecisionAction
        {
            public TransitionTrigger trigger;
            public StateAction state;
            
#pragma warning disable 649
            [SerializeField] 
            internal ActionType type; // assigned by StateNodeEditor
#pragma warning restore 649
        }
        
        public bool enabled;
        public Decision[] decisions;
        public StateNode nextState;
        public DecisionAction[] actions;
    }
}