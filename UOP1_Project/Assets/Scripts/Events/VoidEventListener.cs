using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is listener for Void Events
/// </summary>

public class VoidEventListener : MonoBehaviour
{
	public VoidGameEvent voidGameEvent;
	public UnityEvent OnEventRaised;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (voidGameEvent == null)
		{
			return;
		}
		voidGameEvent.eventRaised += Respond;
	}

	private void OnDisable()
	{
		if (voidGameEvent == null)
		{
			return;
		}
		voidGameEvent.eventRaised -= Respond;
	}

	public void Respond()
	{
		if (OnEventRaised == null)
		{
			return;
		}
		OnEventRaised.Invoke();
	}
}
