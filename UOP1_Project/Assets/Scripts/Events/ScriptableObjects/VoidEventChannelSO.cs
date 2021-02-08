using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : EventChannelBaseSO
{
	private UnityAction OnEventRaised;

	public void RaiseEvent()
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke();
	}

	public void RegisterEvent(UnityAction action) {
		OnEventRaised += action;
	}

	public void UnregisterEvent(UnityAction action)
	{
		OnEventRaised -= action;
	}



}


