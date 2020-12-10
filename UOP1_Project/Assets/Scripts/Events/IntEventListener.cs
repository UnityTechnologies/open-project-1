using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// To use a generic UnityEvent type you must override the generic type.
/// </summary>
[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}

/// <summary>
/// A flexible handler for int events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
/// </summary>
public class IntEventListener : MonoBehaviour
{
	[SerializeField] private IntEventChannelSO _channel = default;

	public IntEvent OnEventRaised;

	private void OnEnable()
	{
		if (_channel != null)
			_channel.OnEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_channel != null)
			_channel.OnEventRaised -= Respond;
	}

	private void Respond(int value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
