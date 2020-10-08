using System;

public class Transition
{
	public Func<bool> Condition { get; }
	public State To { get; }

	public Transition(State to, Func<bool> condition)
	{
		To = to;
		Condition = condition;
	}
}
