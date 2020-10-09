
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
            this.machine = machine;
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
            
            action.logic.machine = machine;
            action.logic.OnUpdate();
        }
        
        internal void CheckTransitions(StateMachine machine)
        {
            // Prioritised transition index
            // [0] transitions have highest priority
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
                    var state = decision.state;
                    if (!state)
                        continue;
                    
                    state.machine = machine;
                    var decided = state.OnDecide();
                    
                    switch (decision.condition)
                    {
                        case DecisionCondition.When:
                            // Nothing changes
                            break;
                        case DecisionCondition.Not:
                            decided = !decided;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    
                    if (!decided)
                    {
                        finalDecision = false;
                    }
                }
                
                foreach (var decision in transition.decisions)
                {
                    var state = decision.state;
                    if (!state)
                        continue;
                    
                    state.AfterDecision(finalDecision);
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
                var decision = machine.previousDecisions[priority];
                
                foreach (var action in transition.actions)
                {
                    // How did I forgot to actually implement this...
                    switch (action.trigger)
                    {
                        case TransitionTrigger.True:
                            if (!decision)
                                continue;
                            break;
                        
                        case TransitionTrigger.False:
                            if (decision)
                                continue;
                            break;
                        
                        default: throw new NotImplementedException();
                    }
                    
                    switch (action.type)
                    {
                        case StateTransition.ActionType.Action:
                            if (!action.state)
                                break;
                            action.state.machine = machine;
                            action.state.OnUpdate();
                            break;
                        
                        case StateTransition.ActionType.ChangeState:
                            machine.ChangeState(transition.nextState);
                            break;
                        
                        default:
                            throw new NotImplementedException();
                    }
                   
                }
            }
        }
    }
}