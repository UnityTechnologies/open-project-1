using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "StaticNpcMovement", menuName = "Characters/Static NPC Movement")]
public class StaticNpcMovementSO : NpcMovementSO
{
	public override NpcMovementData CreateNpcMovementData(GameObject obj)
	{
		return null;
	}

	public override void NpcMovementUpdate(NpcMovementData data)
	{
		// Do nothing
	}
}
