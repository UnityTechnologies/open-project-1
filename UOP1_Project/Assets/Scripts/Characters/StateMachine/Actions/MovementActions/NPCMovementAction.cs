using UnityEngine;
using System.Collections;

public abstract class NPCMovementAction
{
	public abstract void OnUpdate();

	public abstract void OnStateEnter();

	public abstract void OnStateExit();
}
