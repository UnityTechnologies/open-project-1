using UnityEngine.Events;
using UnityEngine;
using Cinemachine;

/// <summary>
/// This class is used for Events that have one cinemachine virtual camera argument.
/// </summary>

[CreateAssetMenu(menuName = "Events/Vcam Event Channel")]
public class VcamEventChannelSO : EventChannelBaseSO
{
	public UnityAction<CinemachineVirtualCamera> OnEventRaised;

	public void RaiseEvent(CinemachineVirtualCamera value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
