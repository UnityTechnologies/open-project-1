using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimationBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		int randomIdle = Random.Range(0, 2);
		animator.SetInteger("RandomIdle", randomIdle);
	}
}
