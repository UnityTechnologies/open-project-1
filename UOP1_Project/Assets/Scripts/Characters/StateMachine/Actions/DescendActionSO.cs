using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Descend", menuName = "State Machines/Actions/Descend")]
public class DescendActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new DescendAction();
}

public class DescendAction : StateAction
{
	//Component references
	private Character _characterScript;

	private float _verticalMovement;
	private const float GRAVITY_MULTIPLIER = 5f;
	private const float MAX_FALL_SPEED = -50f;
	private const float MAX_RISE_SPEED = 100f;

	public override void Awake(StateMachine stateMachine)
	{
		_characterScript = stateMachine.GetComponent<Character>();
	}

	public override void OnStateEnter()
	{
		_verticalMovement = _characterScript.movementVector.y;

		//Prevents a double jump if the player keeps holding the jump button
		//Basically it "consumes" the input
		_characterScript.jumpInput = false;
	}

	public override void OnUpdate()
	{
		_verticalMovement += Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
		_verticalMovement = Mathf.Clamp(_verticalMovement, MAX_FALL_SPEED, MAX_RISE_SPEED);

		_characterScript.movementVector.y = _verticalMovement;
	}
}
