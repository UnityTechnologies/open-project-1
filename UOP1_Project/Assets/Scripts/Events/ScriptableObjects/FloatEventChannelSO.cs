using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>
[CreateAssetMenu(menuName = "Events/Float Event Channel")]
public class FloatEventChannelSO : DescriptionBaseSO
{
	public UnityAction<float> OnEventRaised;
	
	public void RaiseEvent(float value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
