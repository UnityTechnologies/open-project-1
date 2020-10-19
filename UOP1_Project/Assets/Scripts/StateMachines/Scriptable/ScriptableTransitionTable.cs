using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachines.Scriptable
{
    public class ScriptableTransitionTable : ScriptableObject
    {
        [SerializeField] private List<SerializableTransition> _transitions;
        
        [System.Serializable]
        private class SerializableTransition
        {
            public ScriptableState fromState;
            public ScriptableState toState;
            public List<ScriptableCondition> conditions;
        }

        private class Transition : ITransition
        {
            private readonly IState _targetState;
            private readonly IEnumerable<ICondition> _conditions;

            public Transition(IState targetState, IEnumerable<ICondition> conditions)
            {
                _targetState = targetState;
                _conditions = conditions;
            }

            public IState TargetState => _targetState;
            
            public bool Evaluate()
            {
                return _conditions.All(c => c.Value);
            }

            public void OnEnter()
            {
                foreach (var condition in _conditions)
                    condition.OnEnter();
            }

            public void OnUpdate(float deltaTime)
            {
                foreach (var condition in _conditions)
                    condition.OnUpdate(deltaTime);
            }

            public void OnExit()
            {
                foreach (var condition in _conditions)
                    condition.OnExit();
            }
        }

        public IDictionary<IState, IEnumerable<ITransition>> Get()
        {
            Dictionary<IState, List<ITransition>> dictionary = new Dictionary<IState, List<ITransition>>();
            foreach (var transition in _transitions)
            {
                if (!dictionary.ContainsKey(transition.fromState))
                    dictionary.Add(transition.fromState, new List<ITransition>());
                dictionary[transition.fromState].Add(new Transition(transition.toState, transition.conditions));
            }
            
            IDictionary<IState, IEnumerable<ITransition>> result = new Dictionary<IState, IEnumerable<ITransition>>();
            foreach (var pair in dictionary)
            {
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }
    }
}