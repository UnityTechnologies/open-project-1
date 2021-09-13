using UnityEngine;
using UnityEngine.Playables;

public class TimelineBinder : MonoBehaviour
{
	[SerializeField] private PlayableDirector _playableDirector;
	[SerializeField] private GameObject[] _objectsToBind;

	public string[] objectsToBindTags;
	public string[] trackNames;

	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;

	private void OnEnable()
	{
		_playerInstantiatedChannel.OnEventRaised += BindObjects;
	}

	private void OnDisable()
	{
		_playerInstantiatedChannel.OnEventRaised -= BindObjects;
	}

	private void BindObjects(Transform playerTransform)
	{
		_objectsToBind = new GameObject[objectsToBindTags.Length];
		for (int i = 0; i < objectsToBindTags.Length; ++i)
		{
			_objectsToBind[i] = GameObject.FindGameObjectWithTag(objectsToBindTags[i]);
		}

		foreach (var playableAssetOutput in _playableDirector.playableAsset.outputs)
		{
			for (int i = 0; i < objectsToBindTags.Length; ++i)
			{
				if (playableAssetOutput.streamName == trackNames[i])
				{
					_playableDirector.SetGenericBinding(playableAssetOutput.sourceObject, _objectsToBind[i]);
				}
			}
		}
	}
}
