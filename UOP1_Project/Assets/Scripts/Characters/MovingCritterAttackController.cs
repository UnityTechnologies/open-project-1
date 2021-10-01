using UnityEngine;

public class MovingCritterAttackController : MonoBehaviour
{
	[SerializeField] private TransformAnchor _playerTransform;
	[SerializeField] private float _propelFactor = 1.0f; // Propel factor is the proportion of the distance between the critter and the player crossed by the critter during the propel animation
	[SerializeField] private float _propelDuration = 0.2f;

	private float _innerTime = 0.0f;
	private Vector3 _propelTargetVector = default;

	// When the attack starts, the position targeted by the attack is determined and is not changed afterward
	public void SetAttackTarget()
	{
		_propelTargetVector = (_playerTransform.Value.position - transform.position) * _propelFactor / _propelDuration;
	}

	// Trigger the propel movement during the attack
	public void AttackPropelTrigger()
	{
		_innerTime = _propelDuration;
	}

	void Update()
	{
		if (_innerTime > 0)
		{
			transform.position += _propelTargetVector * Time.deltaTime;
			_innerTime -= Time.deltaTime;
		}
	}
}
