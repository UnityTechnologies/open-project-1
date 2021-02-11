using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/step Channel")]
public class StepChannelSO : ScriptableObject
{
	public UnityAction<StepSO> OnEventRaised;
	public void RaiseEvent(StepSO step)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(step);
	}
}
