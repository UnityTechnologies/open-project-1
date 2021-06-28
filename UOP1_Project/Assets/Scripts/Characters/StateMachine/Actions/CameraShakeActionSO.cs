using Cinemachine;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CameraShakeAction", menuName = "State Machines/Actions/Camera Shake")]
public class CameraShakeActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new CameraShakeAction();
}

public class CameraShakeAction : StateAction
{
	private CinemachineImpulseSource _impulseSource;

	public override void Awake(StateMachine stateMachine)
	{
		_impulseSource = stateMachine.GetComponent<CinemachineImpulseSource>();

		if (_impulseSource == null)
		{
			Debug.LogError("Please Add CinemachineImpulseSource to GameObject");
		}
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateEnter()
	{
		if (_impulseSource == null)
		{
			return;
		}

		_impulseSource.GenerateImpulse();
	}
	
	public override void OnStateExit()
	{
	}
}
