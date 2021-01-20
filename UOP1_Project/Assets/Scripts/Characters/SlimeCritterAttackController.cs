using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCritterAttackController : MonoBehaviour
{
	// Reference of the player transform to compute the propel target position
	[SerializeField]
	private TransformAnchor _playerTransform;

	// Propel factor is the proportion of the distance between the critter and the player crossed by the critter during the propel animation
	[SerializeField]
	private float _propelFactor = 1.0f;

	// Duration of the propel section of the animation.
	[SerializeField]
	private float _propelDuration = 0.2f;

	private float _innerTime = 0.0f;
	private Vector3 _propelTargetVector = default;

	// When the attack starts, the position targeted by the attack is determined and is not changed afterward
	public void SetAttackTarget()
	{
		_propelTargetVector = (_playerTransform.Transform.position - transform.position) * _propelFactor / _propelDuration;
	}

	// Trigger the propel movement during the attack
	public void AttackPropelTrigger()
	{
		_innerTime = _propelDuration;
	}

	// Update is called once per frame
	void Update()
	{
		if (_innerTime > 0)
		{
			transform.position += _propelTargetVector * Time.deltaTime;
			_innerTime -= Time.deltaTime;
		}
	}
}
