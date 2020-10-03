
using System;
using UnityEngine;

namespace AV.Logic
{
    [CreateAssetMenu(menuName = "State Machine/State Node")]
    public class StateNode : ScriptableState
    {
        [Serializable]
        public struct ActionUsage
        {
            public StateTrigger trigger;
            public StateAction logic;
        }
        public ActionUsage[] actions;
        public StateTransition[] transitions;

        internal void UpdateActions(StateMachine machine)
        {
            foreach (var action in actions)
            {
                if (action.trigger != StateTrigger.OnUpdate)
                    continue;

                UpdateAction(action);
            }
        }

        private void UpdateAction(ActionUsage action)
        {
            if (!action.logic)
            {
                Debug.Log($"{name} (StateNode) has unassigned action.", this);
                return;
            }
                
            action.logic.BeginUpdate(machine);
        }

        internal void CheckTransitions(StateMachine machine)
        {
            // Prioritised transition index
            // [0] transitions have highest priority
            // Example:
            // We see player and start chase, but if we hungry and see food - we chase food instead
            // If watching for player is higher on the list, we'll not care about food while chasing player
            // TODO: Make priority matter
            var priority = int.MaxValue;
            
            for (var i = 0; i < transitions.Length; i++)
            {
                var transition = transitions[i];

                if (!transition.enabled)
                    continue;

                var finalDecision = true;
                foreach (var decision in transition.decisions)
                {
                    if (!decision)
                        continue;
                    
                    decision.machine = machine;
                    var decided = decision.OnDecide();

                    if (!decided)
                    {
                        finalDecision = false;
                        decision.OnFalse();
                    }
                }
                
                foreach (var decision in transition.decisions)
                {
                    if (finalDecision)
                        decision.OnTrue();
                    else
                        decision.OnFalse();
                }

                // Skip if decision has not changed
                if (finalDecision == machine.previousDecisions[i])
                    continue;

                machine.previousDecisions[i] = finalDecision;

                if (priority > i)
                    priority = i;
            }

            if (priority != int.MaxValue)
            {
                var transition = transitions[priority];

                foreach (var action in transition.actions)
                {
                    if (!action.state)
                        continue;
                    
                    switch (action.type)
                    {
                        case StateTransition.StateType.Node:
                            machine.EnterState(action.state as StateNode);
                            break;
                        
                        default:
                            action.state.BeginUpdate(machine);
                            break;
                    }
                }
            }
        }

        internal override void BeginUpdate(StateMachine machine)
        {
            // Unused
        }
    }
}