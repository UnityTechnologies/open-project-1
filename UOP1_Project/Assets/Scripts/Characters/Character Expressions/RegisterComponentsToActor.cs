using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterComponentsToActor : MonoBehaviour
{
	[SerializeField] private ActorSO _actor;

	private void OnEnable()
	{
		if (_actor)
		{
			Animator animator = GetComponent<Animator>();
			SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();

			if (animator != null)
				_actor.RegisterAnimator(animator);

			if (smr != null)
				_actor.RegisterSkinnedMeshRenderer(smr);
		}
	}

	private void OnDisable()
	{
		if (_actor)
		{
			Animator animator = GetComponent<Animator>();
			SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();

			if (animator != null)
				_actor.UnregisterAnimator(animator);

			if (smr != null)
				_actor.UnregisterSkinnedMeshRenderer(smr);
		}
	}
}
