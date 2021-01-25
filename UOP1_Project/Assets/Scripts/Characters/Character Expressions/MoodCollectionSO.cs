using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

// The moods that characters can have
public enum ActorMood { NONE, NEUTRAL, HAPPY, SAD, SCARED, ANGRY, BORED, EXPLAINING, RANDOM }

[System.Serializable]
public class LocalizedPhonemeSet : LocalizedAsset<PhonemeSetSO> { }

// Mood sets should be specific to the Actor
[CreateAssetMenu(menuName = "Actor/New Mood Collection")]
public class MoodCollectionSO : ScriptableObject
{
	public ActorSO Actor;
	public ActorMood Mood;
	public Texture2D EyeState_Open;
	public Texture2D EyeState_MidBlink;
	public Texture2D EyeState_Closed;
	public LocalizedPhonemeSet LocalizedPhonemeSet = default;
	public List<string> AnimatorClipTitles;

	private bool _hashesGenerated = false;
	private List<int> _animatorTitleHashes;

	public List<int> AnimatorTitleHashes
	{
		get
		{
			if (!_hashesGenerated)
				GenerateHashes();

			return _animatorTitleHashes;
		}
	}

	// Generate animator title hashes for efficient runtime lookup
	public void GenerateHashes()
	{
		_animatorTitleHashes.Clear();

		foreach (string s in AnimatorClipTitles)
		{
			int hash = Animator.StringToHash(s);

			_animatorTitleHashes.Add(hash);
		}
	}
}
