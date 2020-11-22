using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(menuName = "State Machines/Actions/Play AudioCue")]
public class PlayAudioCueActionSO : StateActionSO
{
	[SerializeField] private AudioCueSO _audioCue = default;
	[SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
	[SerializeField] private AudioConfigurationSO _audioConfiguration = default;

	protected override StateAction CreateAction() => new PlayAudioCueAction(_audioCue, _audioCueEventChannel, _audioConfiguration);
}

public class PlayAudioCueAction : StateAction
{
	private AudioCueEventChannelSO _audioCueEventChannel;
	private AudioCueSO _audioCue;
	private Transform _stateMachineTransform;
	private AudioConfigurationSO _audioConfiguration;

	public PlayAudioCueAction(AudioCueSO audioCue, AudioCueEventChannelSO audioCueEventChannel, AudioConfigurationSO audioConfiguration)
	{
		_audioCue = audioCue;
		_audioCueEventChannel = audioCueEventChannel;
		_audioConfiguration = audioConfiguration;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_stateMachineTransform = stateMachine.transform;
	}

	public override void OnUpdate() { }

	public override void OnStateEnter()
	{
		_audioCueEventChannel.RaiseEvent(_audioCue, _audioConfiguration, _stateMachineTransform.position);
	}
}
