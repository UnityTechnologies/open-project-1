using UnityEngine;

public class SlimeCritterAttackController : MonoBehaviour
{
	// Duration of the propel section of the animation.
	[SerializeField]
	private float _propelDuration = 0.2f;

	[SerializeField]
	private float _distanceFromTarget = 0.7f;

	private float _innerTime = 0.0f;
	private Vector3 _propelTargetVector = default;

	private Critter _critter;

	private void Awake()
	{
		_critter = GetComponent<Critter>();
	}

	// When the attack starts, the position targeted by the attack is determined and is not changed afterward
	public void SetAttackTarget()
	{
		_propelTargetVector = Vector3.zero;
		if (_critter.currentTarget != null)
		{
			Vector3 dir = _critter.currentTarget.gameObject.transform.position - transform.position;
			// Target a position 0.3f far from player center (approximately the player model radius) to avoid entering in the player. 
			_propelTargetVector = (dir - dir.normalized * _distanceFromTarget) / _propelDuration;

			//Prevent critter to propel backward if he is too close to the player.
			if (Vector3.Dot(dir, _propelTargetVector) < 0)
			{
				_propelTargetVector = Vector3.zero;
			}
		}
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
