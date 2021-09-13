using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one NPCMovementConfigSO argument.
/// </summary>
[CreateAssetMenu(menuName = "Events/NPC Movement Event Channel")]
public class NPCMovementEventChannelSO : DescriptionBaseSO
{
	public UnityAction<NPCMovementConfigSO> OnEventRaised;

	public void RaiseEvent(NPCMovementConfigSO value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
