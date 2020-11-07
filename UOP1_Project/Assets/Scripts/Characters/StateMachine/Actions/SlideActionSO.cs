using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO
{
	[Tooltip("Sliding speed on the XZ plane.")]
	[SerializeField] private float _slideSpeed = 6f;

	protected override StateAction CreateAction() => new SlideAction(_slideSpeed);
}

public class SlideAction : StateAction
{
	//Component references
	private Character _characterScript;

	private float _slideSpeed;

	public SlideAction(float slideSpeed)
	{
		_slideSpeed = slideSpeed;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_characterScript = stateMachine.GetComponent<Character>();
	}

	public override void OnUpdate()
	{
		Vector3 hitNormal = _characterScript.lastHit.normal;
		_characterScript.movementVector.x = (1f - hitNormal.y) * hitNormal.x * _slideSpeed;
		_characterScript.movementVector.z = (1f - hitNormal.y) * hitNormal.z * _slideSpeed;
	}

	public override void OnStateExit()
	{
		_characterScript.movementVector = Vector3.zero;
	}
}
