using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>

[CreateAssetMenu(menuName = "Events/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
	public UnityAction<int> OnEventRaised;
	public void RaiseEvent(int value)
	{
		OnEventRaised.Invoke(value);
	}
}
