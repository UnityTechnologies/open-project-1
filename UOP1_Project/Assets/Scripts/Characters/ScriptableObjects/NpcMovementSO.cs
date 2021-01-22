using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class NpcMovementSO : ScriptableObject
{
	public abstract NpcMovementData CreateNpcMovementData(GameObject obj);

	public abstract void NpcMovementUpdate(NpcMovementData data);

}

public abstract class NpcMovementData
{

}
