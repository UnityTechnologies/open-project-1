using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

/// <summary>
/// This Action handles horizontal movement while in the air, keeping momentum, simulating air resistance, and accelerating towards the desired speed.
/// </summary>
[CreateAssetMenu(fileName = "AerialMovement", menuName = "State Machines/Actions/Aerial Movement")]
public class AerialMovementActionSO : StateActionSO
{
	public float Speed => _speed;
	public float Acceleration => _acceleration;

	[Tooltip("Desired horizontal movement speed while in the air")]
	[SerializeField] [Range(0.1f, 100f)] private float _speed = 10f;
	[Tooltip("The acceleration applied to reach the desired speed")]
	[SerializeField] [Range(0.1f, 100f)] private float _acceleration = 20f;

	protected override StateAction CreateAction() => new AerialMovementAction();
}

public class AerialMovementAction : StateAction
{
	private new AerialMovementActionSO OriginSO => (AerialMovementActionSO)base.OriginSO;

	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		Vector3 velocity = _protagonist.movementVector;
		Vector2 horizontalInput = new Vector2(_protagonist.movementInput.x, _protagonist.movementInput.z);
		float acceleration = OriginSO.Acceleration;
		float speed = OriginSO.Speed;

		Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
		SetHorizontalVelocity(ref horizontalVelocity, horizontalInput, acceleration, speed);
		velocity.x = horizontalVelocity.x;
		velocity.z = horizontalVelocity.y;

		_protagonist.movementVector = velocity;
	}

	private void SetHorizontalVelocity(ref Vector2 horizontalVelocity, Vector2 horizontalInput, float acceleration, float targetSpeed)
	{
		float inputMagnitude = horizontalInput.magnitude;
		if (Mathf.Approximately(inputMagnitude, 0f))
		{
			ApplyAirResistance(ref horizontalVelocity);
		}
		else
		{
			targetSpeed *= inputMagnitude;

			horizontalVelocity += horizontalInput * acceleration;

			// Apply a speed limit
			float horizontalSpeed = horizontalVelocity.magnitude;
			if (targetSpeed < horizontalSpeed)
				horizontalVelocity = horizontalVelocity / horizontalSpeed * targetSpeed;
		}
	}

	private void ApplyAirResistance(ref Vector2 vector)
	{
		if (Mathf.Approximately(vector.sqrMagnitude, 0))
			return;
		vector -= vector.normalized * Protagonist.AIR_RESISTANCE * Time.deltaTime;
	}
}
