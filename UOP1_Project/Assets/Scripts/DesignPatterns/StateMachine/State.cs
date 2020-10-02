using System;
using System.Collections.Generic;

public abstract class State
{
	public abstract void Tick();
	public abstract void OnEnter();
	public abstract void OnExit();

	private readonly List<Transition> transitions = new List<Transition>();

	public void AddTransition(State toState, Func<bool> predicateCondition)
	{
		transitions.Add(new Transition(toState, predicateCondition));
	}

	public List<Transition> Transitions => transitions;
}