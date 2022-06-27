using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO<SlideAction> { }

public class SlideAction : StateAction
{
	/// <summary>
	/// Calculated by Cos(90-3 degrees) because 3 degrees is the problem begins
	/// </summary>
	const float FLAT_NORMAL_THRESHOLD = 0.05233595624f;
	const float ADD_NORMAL_FACTOR = 0.1f;

	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		float speed = -Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * .4f;
		Vector3 hitNormal = _protagonist.lastHit.normal;
		Vector3 biTangent = Vector3.Cross(hitNormal, Vector3.up);
		Vector3 slideDirection = Vector3.Cross(hitNormal, biTangent);

		// Check if the normal is close to flat/when the surface is too steep
		if (Mathf.Abs(Vector3.Dot(hitNormal, Vector3.up)) < FLAT_NORMAL_THRESHOLD)
		{
			// Moving downwards now may cause the CharacterController to get stuck
			// Adding a small factor of the normal to the slideDirection fixes this issue
			slideDirection += hitNormal * ADD_NORMAL_FACTOR;
			slideDirection.Normalize();
		}

		//Trick below has been commented because it was pushing the character "into" the ground much too often,
		//producing a collision, which would result in the character being stuck while in the Sliding state

		//Vector3 slidingMovement = _protagonist.movementVector;
		//// Cheap way to avoid overshooting the character, which causes it to move away from the slope
		//if (Mathf.Sign(slideDirection.x) == Mathf.Sign(slidingMovement.x))
		//	slideDirection.x *= 0.5f;
		//if (Mathf.Sign(slideDirection.z) == Mathf.Sign(slidingMovement.z))
		//	slideDirection.z *= 0.5f;

		//slidingMovement += slideDirection * speed;

		_protagonist.movementVector = slideDirection * speed;
	}
}
