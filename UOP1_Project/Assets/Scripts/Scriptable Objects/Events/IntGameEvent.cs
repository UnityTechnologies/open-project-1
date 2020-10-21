using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "IntGameEvent", menuName = "Game Event/Int")]
public class IntGameEvent : ScriptableObject
{
	public UnityAction<int> eventRaised;
	public void Raise(int value)
	{
		eventRaised.Invoke(value);
	}
}
