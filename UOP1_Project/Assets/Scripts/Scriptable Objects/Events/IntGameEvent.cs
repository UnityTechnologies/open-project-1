using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one Int argument (Example: Achievement Unlock Event)
/// </summary>

[CreateAssetMenu(fileName = "IntGameEvent", menuName = "Game Event/Int")]
public class IntGameEvent : ScriptableObject
{
	public UnityAction<int> eventRaised;
	public void Raise(int value)
	{
		eventRaised.Invoke(value);
	}
}
