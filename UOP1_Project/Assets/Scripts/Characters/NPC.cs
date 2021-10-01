using System.Collections;
using UnityEngine;

public enum NPCState
{
	Idle = 0,
	Walk,
	Talk
};

public class NPC : MonoBehaviour
{
	public NPCState npcState; //This is checked by conditions in the StateMachine
	public GameObject[] talkingTo;

	public void SwitchToWalkState()
	{
		StartCoroutine(WaitBeforeSwitch());
	}

	IEnumerator WaitBeforeSwitch()
	{
		int wait_time = Random.Range(0, 4);
		yield return new WaitForSeconds(wait_time);
		npcState = NPCState.Walk;
	}
}

