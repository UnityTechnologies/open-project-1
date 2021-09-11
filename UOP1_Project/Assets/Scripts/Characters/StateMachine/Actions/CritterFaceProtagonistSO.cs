using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CritterFaceProtagonist", menuName = "State Machines/Actions/Critter Face Protagonist")]
public class CritterFaceProtagonistSO : StateActionSO
{
	public TransformAnchor playerAnchor;
	protected override StateAction CreateAction() => new CritterFaceProtagonist();
}

public class CritterFaceProtagonist : StateAction
{
	TransformAnchor _protagonist;
	Transform _actor;
	public override void Awake(StateMachine stateMachine)
	{
		_actor = stateMachine.transform;
		_protagonist = ((CritterFaceProtagonistSO)OriginSO).playerAnchor;
	}

	public override void OnUpdate()
	{
		if (_protagonist.isSet)
		{
			Vector3 relativePos = _protagonist.Value.position - _actor.position;
			relativePos.y = 0f; // Force rotation to be only on Y axis.

			Quaternion rotation = Quaternion.LookRotation(relativePos);
			_actor.rotation = rotation;
		}
	}

	public override void OnStateEnter()
	{

	}
}
