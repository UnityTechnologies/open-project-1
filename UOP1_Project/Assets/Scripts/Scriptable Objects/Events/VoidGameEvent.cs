using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(fileName = "VoidGameEvent", menuName = "Game Event/Void")]
public class VoidGameEvent : ScriptableObject
{
	public UnityAction eventRaised;
	public void Raise()
	{
		if (eventRaised != null)
			eventRaised.Invoke();
	}
}


