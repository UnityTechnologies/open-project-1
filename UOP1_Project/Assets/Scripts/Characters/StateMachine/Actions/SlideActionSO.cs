using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO<SlideAction>
{
	[Tooltip("Sliding speed on the XZ plane.")]
	public float slideSpeed = 10f;
}

public class SlideAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private SlideActionSO _originSO => (SlideActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		Vector3 hitNormal = _protagonistScript.lastHit.normal;
		_protagonistScript.movementVector.x = (1f - hitNormal.y) * hitNormal.x * _originSO.slideSpeed;
		_protagonistScript.movementVector.z = (1f - hitNormal.y) * hitNormal.z * _originSO.slideSpeed;
	}

	public override void OnStateExit()
	{
		_protagonistScript.movementVector = Vector3.zero;
	}
}
