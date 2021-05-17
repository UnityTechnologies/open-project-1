using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TownsfolkState { Idle = 0, Walk, Talk };

public class Townsfolk : MonoBehaviour
{
	public TownsfolkState townsfolkState; //This is checked by conditions in the StateMachine
	public GameObject[] talkingTo;
}

