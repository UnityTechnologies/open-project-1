using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
public class HorizontalMoveActionSO : StateActionSO
{
	[Tooltip("Horizontal XZ plane speed multiplier")] [SerializeField] private float _speed = 8f;

	protected override StateAction CreateAction() => new WalkAction(_speed);
}

public class WalkAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;

	private float _speed;

	public WalkAction(float speed)
	{
		_speed = speed;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		_protagonistScript.movementVector.x = _protagonistScript.movementInput.x * _speed;
		_protagonistScript.movementVector.z = _protagonistScript.movementInput.z * _speed;
	}
}
