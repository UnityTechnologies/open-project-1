using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one transform argument.
/// </summary>

[CreateAssetMenu(menuName = "Events/Transform Event Channel")]
public class TransformEventChannelSO : EventChannelBaseSO
{
	public UnityAction<Transform> OnEventRaised;

	public void RaiseEvent(Transform value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
