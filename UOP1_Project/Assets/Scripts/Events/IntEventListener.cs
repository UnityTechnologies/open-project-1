using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// To use a generic UnityEvent type you must override the class type
/// </summary>
[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}

/// <summary>
/// This class is listener for Int Events
/// </summary>
public class IntEventListener : MonoBehaviour
{
	public IntGameEvent intGameEvent;
	public IntEvent OnEventRaised;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (intGameEvent == null)
		{
			return;
		}
		intGameEvent.eventRaised += Respond;
	}

	private void OnDisable()
	{
		if (intGameEvent == null)
		{
			return;
		}
		intGameEvent.eventRaised -= Respond;
	}

	public void Respond(int value)
	{
		if (OnEventRaised == null)
		{
			return;
		}
		OnEventRaised.Invoke(value);
	}
}
