using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MainCharacterBinder : MonoBehaviour
{
	[SerializeField] private PlayableDirector _playableDirector;
	[SerializeField] private Animator _objectToBind;
	public string trackName;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;

	private void OnEnable()
	{
		_playerInstantiatedChannel.OnEventRaised += BindPlayer;
	}
	private void OnDisable()
	{
		_playerInstantiatedChannel.OnEventRaised -= BindPlayer;
	}

	private void BindPlayer(Transform playerTransform)
	{
		_objectToBind = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		foreach (var playableAssetOutput in _playableDirector.playableAsset.outputs)
		{
			if (playableAssetOutput.streamName == trackName)
			{
				_playableDirector.SetGenericBinding(playableAssetOutput.sourceObject, _objectToBind);
			}
		}
	}
}
