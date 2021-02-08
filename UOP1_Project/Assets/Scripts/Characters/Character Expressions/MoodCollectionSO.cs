using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

// The moods that characters can have
public enum ActorMood      { NONE, NEUTRAL, HAPPY, SAD, SCARED, ANGRY, BORED, EXPLAINING, RANDOM }

// Actor eyes / mouths can either be 2D (textures) or 3D (blendshapes) ... can mix and match
// ex: 2D eyes and 3D mouth
public enum ActorEyeType   { Undefined, TwoD, ThreeD }
public enum ActorMouthType { Undefined, TwoD, ThreeD }

[System.Serializable]
public class LocalizedPhonemeSet : LocalizedAsset<PhonemeSetSO> { }

// Mood sets should be specific to the Actor
[CreateAssetMenu(menuName = "Actor/New Mood Collection")]
public class MoodCollectionSO : ScriptableObject
{
	[Header("General Settings")]
	public ActorSO        Actor;
	public ActorMood      Mood;
	public ActorEyeType   EyeType;
	public ActorMouthType MouthType;

	// TODO: Write an editor script that only displays the appropriate properties for the
	// ActorEyeType and ActorMouthType
	[Header("Eye and Mouth Settings")]
	// 2D Eyes
	public Texture2D EyeState_Open_2D;
	public Texture2D EyeState_MidBlink_2D;
	public Texture2D EyeState_Closed_2D;

	// 3D Eyes
	public string            Blink_3D;
	public float             MoodTransitionTime = 1.0f;
	public List<BlendTarget> EyeMoodTargets;
	public List<BlendTarget> MouthMoodTargets;
	
	// 2D & 3D Mouths
	public LocalizedPhonemeSet LocalizedPhonemeSet = default;

	// Animator title applies for both 2D / 3D characters
	public List<string> AnimatorClipTitles;

	// Private
	private bool _hashesGenerated = false;
	private List<int> _animatorTitleHashes;

	// Public Accessor
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
