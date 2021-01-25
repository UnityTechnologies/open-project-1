using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterAnimatorToActor : MonoBehaviour
{
	[SerializeField] private ActorSO _actor;

	private void OnEnable()
	{
		if (_actor)
		{
			Debug.Log("ACTOR REGISTERED");
			_actor.RegisterAnimator(GetComponent<Animator>());
		}
	}

	private void OnDisable()
	{
		if (_actor)
		{
			Debug.Log("ACTOR UNREGISTERED");
			_actor.UnregisterAnimator(GetComponent<Animator>());
		}
	}
}
