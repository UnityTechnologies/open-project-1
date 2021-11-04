using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/step Channel")]
public class StepChannelSO : DescriptionBaseSO
{
	public UnityAction<StepSO> OnEventRaised;

	public void RaiseEvent(StepSO step)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(step);
	}
}
