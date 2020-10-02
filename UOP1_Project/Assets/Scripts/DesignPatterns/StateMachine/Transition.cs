using System;

public class Transition{
	public Func<bool> Condition {get; }
	public IState To { get; }

	public Transition(IState to, Func<bool> condition)
	{
		To = to;
		Condition = condition;
	}
}