using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play AudioCue")]
public class PlayAudioCueActionSO : StateActionSO<PlayAudioCueAction>
{
	public AudioCueSO audioCue = default;
	public AudioCueEventChannelSO audioCueEventChannel = default;
	public AudioConfigurationSO audioConfiguration = default;
}

public class PlayAudioCueAction : StateAction
{
	private Transform _stateMachineTransform;

	private PlayAudioCueActionSO _originSO => (PlayAudioCueActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		_stateMachineTransform = stateMachine.transform;
	}

	public override void OnUpdate() { }

	public override void OnStateEnter()
	{
		_originSO.audioCueEventChannel.RaisePlayEvent(_originSO.audioCue, _originSO.audioConfiguration, _stateMachineTransform.position);
	}
}
