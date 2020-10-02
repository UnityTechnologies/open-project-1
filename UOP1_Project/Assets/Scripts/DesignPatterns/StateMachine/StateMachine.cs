using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	private IState _currentState;
	
	private List<Transition> _transitionsForCurrentState = new List<Transition>();
	private readonly List<Transition> _transitionsFromAnyState = new List<Transition>();
	
	private readonly Dictionary<Type, List<Transition>> _allTransitionsForType = new Dictionary<Type,List<Transition>>();

	private static List<Transition> _emptyTransitionsList = new List<Transition>();

	public void Tick()
	{
		Transition transition = GetTransitionIfAvailable(_currentState);
		if (transition != null)
		{
			SetState(transition.To);
		}

		_currentState?.Tick();
	}

	public void SetState(IState state)
	{
		if (state == _currentState)
		{
			return;
		}
		
		_currentState?.OnExit();
		_currentState = state;
  
		_allTransitionsForType.TryGetValue(_currentState.GetType(), out _transitionsForCurrentState);
		if (_transitionsForCurrentState == null)
		{
			_transitionsForCurrentState = _emptyTransitionsList;
		}
			
  
		_currentState.OnEnter();
	}

	public void AddTransition(IState from, IState to, Func<bool> predicateCondition)
	{
		if (_allTransitionsForType.TryGetValue(from.GetType(), out var transitions) == false)
		{
			transitions = new List<Transition>();
			_allTransitionsForType[from.GetType()] = transitions;
		}
  
		transitions.Add(new Transition(to, predicateCondition));
	}

	public void AddAnyTransition(IState state, Func<bool> predicate)
	{
		_transitionsFromAnyState.Add(new Transition(state, predicate));
	}

	private Transition GetTransitionIfAvailable(IState currentState)
	{
		Transition transitionIfAny = GetNextTransitionFromList(_transitionsFromAnyState);

		if (transitionIfAny == null || transitionIfAny.To == currentState)
		{
			transitionIfAny = GetNextTransitionFromList(_transitionsForCurrentState);
		}

		return transitionIfAny;
	}

	private Transition GetNextTransitionFromList(List<Transition> transitions)
	{
		foreach (Transition transition in transitions)
		{
			if (transition.Condition())
			{
				return transition;
			}
		}

		return null;
	}
}