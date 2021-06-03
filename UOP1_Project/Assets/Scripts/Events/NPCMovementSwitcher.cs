using UnityEngine;
using System.Collections;

public class NPCMovementSwitcher : MonoBehaviour
{
	[SerializeField] private NPCMovementEventChannelSO _movementChannel;

	[SerializeField] private NPCMovementConfigSO _movementConfig;

	[ContextMenu("Trigger NPC Movement switch")]
	public void SwitchMovement()
	{
		if (_movementChannel != null && _movementConfig != null)
		{
			_movementChannel.RaiseEvent(_movementConfig);
		}
	}
}
