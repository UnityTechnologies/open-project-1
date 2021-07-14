using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState { Idle = 0, Walk, Talk};

public class NPC : MonoBehaviour
{
	public NPCState npcState; //This is checked by conditions in the StateMachine
	public GameObject[] talkingTo;
}

