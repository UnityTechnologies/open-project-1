using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
	public abstract class TransitionTable<T> : TransitionTableBase
	{
		public State<T> InitialState { get; private set; }
		private Dictionary<State<T>, List<Transition<T>>> _transitions = new Dictionary<State<T>, List<Transition<T>>>();
		private List<Transition<T>> _anyTransitions = new List<Transition<T>>();

		//Verify if I can transit from 'fromState' to 'toState'
		public bool CanTransit(State<T> fromState, out State<T> toState, T dataModel)
		{
			return HasStateToStateTransition(fromState, out toState, dataModel)
				|| HasAnyStateToStateTransition(fromState, out toState, dataModel);
		}

		private bool HasStateToStateTransition(State<T> fromState, out State<T> toState, T dataModel)
		{
			toState = null;
			return _transitions.ContainsKey(fromState) && HasValidCondition(out toState, _transitions[fromState], dataModel);
		}

		private bool HasAnyStateToStateTransition(State<T> fromState, out State<T> toState, T dataModel)
		{
			return HasValidCondition(out toState, _anyTransitions, dataModel) && fromState != toState;
		}

		private bool HasValidCondition(out State<T> nextState, List<Transition<T>> transitions, T dataModel)
		{
			nextState = null;

			foreach (var transition in transitions)
				if (transition.ConditionIsMet(dataModel))
				{
					nextState = transition.NextState;
					return true;
				}

			return false;
		}

		#region Inspector utils

		private static Type _stateTypeCache;
		private static Type _conditionTypeCache;

		public override Type GetStateType()
		{
			if (_stateTypeCache == null)
				_stateTypeCache = typeof(State<T>);

			return _stateTypeCache;
		}

		public override Type GetConditionType()
		{
			if (_conditionTypeCache == null)
				_conditionTypeCache = typeof(Condition<T>);

			return _conditionTypeCache;
		}

		//called by TransitionTableBase only once when we enter PlayMode or in Built
		protected override void Initialize(Object initialState, List<StateToStateTransition> stateToState, List<AnyStateToStateTransition> anyToState)
		{
			InitialState = (State<T>)initialState;

			foreach (StateToStateTransition entry in stateToState)
			{
				if (entry.IsValid())
				{
					State<T> from = (State<T>)entry.FromState;
					State<T> to = (State<T>)entry.ToState;
					Condition<T> condition = (Condition<T>)entry.Condition;

					AddTransition(from, new Transition<T>(to, condition, entry.ConditionResult));
				}
			}

			foreach (AnyStateToStateTransition entry in anyToState)
			{
				if (entry.IsValid())
				{
					State<T> to = (State<T>)entry.ToState;
					Condition<T> condition = (Condition<T>)entry.Condition;

					AddTransition(new Transition<T>(to, condition, entry.ConditionResult));
				}
			}
		}

		private void AddTransition(State<T> from, Transition<T> transition)
		{
			if (_transitions.TryGetValue(from, out var transitions) == false)
			{
				transitions = new List<Transition<T>>();
				_transitions[from] = transitions;
			}

			transitions.Add(transition);
		}

		private void AddTransition(Transition<T> transition)
		{
			_anyTransitions.Add(transition);
		}

		#endregion
	}
}
