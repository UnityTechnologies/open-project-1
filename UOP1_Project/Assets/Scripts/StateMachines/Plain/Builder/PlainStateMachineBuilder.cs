using System;
using System.Collections.Generic;

namespace StateMachines.Plain.Builder
{
    public class PlainStateMachineBuilder
    {
        private Dictionary<IState, List<ITransition>> _transitions = new Dictionary<IState, List<ITransition>>();
        
        private IState _currentState;
        private IState _defaultState;
        
        public PlainStateMachineBuilder Default(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            _defaultState = state;
            return this;
        }
        
        public PlainStateMachineBuilder From(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            _currentState = state;
            return this;
        }
        
        public PlainStateMachineBuilder To(IState state, Func<bool> condition)
        {
            if (_currentState == null)
                throw new Exception("Invalid builder state. Call From<T>() before using To<T>()");
            if (!_transitions.ContainsKey(_currentState))
                _transitions.Add(_currentState, new List<ITransition>());
            var transition = new PlainTransition(_currentState, condition);
            _transitions[state].Add(transition);
            return this;
        }
        
        
        public IStateMachine Build()
        {
            IDictionary<IState, IEnumerable<ITransition>> transitionsMap = new Dictionary<IState, IEnumerable<ITransition>>();
            foreach (var pair in _transitions)
            {
                transitionsMap.Add(pair.Key, pair.Value);
            }
            return new StateMachine(_defaultState, transitionsMap);
        }
    }
}