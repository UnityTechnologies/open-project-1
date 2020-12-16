using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Vector3 Array Channel")]
public class Vector3ArrayChannelSO : EventChannelBaseSO
{
	public UnityAction<Vector3[]> OnEventRaised;

	public void RaiseEvent(Vector3[] vectorArray)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(vectorArray);
	}
}
