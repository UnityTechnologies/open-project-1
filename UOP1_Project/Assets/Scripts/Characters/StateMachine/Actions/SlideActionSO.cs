using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO<SlideAction> { }

public class SlideAction : StateAction
{
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		Vector3 velocity = _protagonistScript.movementVector;
		float speed = -Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * Time.deltaTime;

		Vector3 hitNormal = _protagonistScript.lastHit.normal;
		var vector = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
		Vector3.OrthoNormalize(ref hitNormal, ref vector);

		// Cheap way to avoid overshooting the character, which causes it to move away from the slope
		if (Mathf.Sign(vector.x) == Mathf.Sign(velocity.x))
			vector.x *= 0.5f;
		if (Mathf.Sign(vector.z) == Mathf.Sign(velocity.z))
			vector.z *= 0.5f;

		velocity += vector * speed;

		_protagonistScript.movementVector = velocity;
	}
}
