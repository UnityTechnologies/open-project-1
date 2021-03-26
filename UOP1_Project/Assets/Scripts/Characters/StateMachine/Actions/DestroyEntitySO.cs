using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DestroyEntity", menuName = "State Machines/Actions/Destroy Entity")]
public class DestroyEntitySO : StateActionSO
{
	protected override StateAction CreateAction() => new DestroyEntity();
}

public class DestroyEntity : StateAction
{
	private GameObject _gameObject;

	public override void Awake(StateMachine stateMachine)
	{
		_gameObject = stateMachine.gameObject;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		GameObject.Destroy(_gameObject);
	}
}
