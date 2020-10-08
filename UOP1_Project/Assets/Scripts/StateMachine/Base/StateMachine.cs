using System;
using System.Collections.Generic;
using UnityEngine;

namespace AV.Logic
{
    [AddComponentMenu("Logic/State Machine")]
    public partial class StateMachine : MonoBehaviour
    {
        // TODO: Support multiple states at once.
        // TODO: StateFlow class for storing and visualizing states relation in a nice editor.

        public StateNode currentState;
        [HideInInspector]
        internal bool[] previousDecisions;
        [HideInInspector]
        internal new Transform transform;

        // TODO: Runtime data inspector (This one is hard as now all our data is inside static generic dictionaries...)

        public void Initialize()
        {
            transform = base.transform;

            if (!currentState)
                return;

            InitializeTransitionsData(currentState);
            CheckStateTriggers(currentState, StateTrigger.OnStart);
        }

        private void InitializeTransitionsData(StateNode state)
        {
            previousDecisions = new bool[state.transitions.Length];
        }
        
        public void Run()
        {
            if (!currentState)
                return;

            currentState.machine = this;
            currentState.UpdateActions(this);
            currentState.CheckTransitions(this);
        }

        public void EnterState(StateNode nextState)
        {
            if (!nextState)
                return;
            
            CheckStateTriggers(currentState, StateTrigger.OnExit);
                
            currentState = nextState;

            InitializeTransitionsData(nextState);
            CheckStateTriggers(nextState, StateTrigger.OnEnter);
        }
        
        private void CheckStateTriggers(StateNode state, StateTrigger trigger)
        {
            foreach (var action in state.actions)
            {
                if (action.trigger != trigger || !action.logic)
                    continue;
                action.logic.BeginUpdate(this);
            }
        }
    }
}