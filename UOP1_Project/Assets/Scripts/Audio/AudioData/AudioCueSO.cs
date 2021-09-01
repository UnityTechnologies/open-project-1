using Assets.Scripts.Audio;
using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A collection of audio clips that are played in parallel, and support randomisation.
/// </summary>
[CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
public class AudioCueSO : ScriptableObject
{
	public bool looping = false;
	[SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

	public VisualisableAudioClip[] GetClips()
	{
		int numberOfClips = _audioClipGroups.Length;
		VisualisableAudioClip[] resultingClips = new VisualisableAudioClip[numberOfClips];

		for (int i = 0; i < numberOfClips; i++)
		{
			resultingClips[i] = _audioClipGroups[i].GetNextClip();
		}

		return resultingClips;
	}
}


/// <summary>
/// Represents a group of AudioClips that can be treated as one, and provides automatic randomisation or sequencing based on the <c>SequenceMode</c> value.
/// </summary>
[Serializable]
public class AudioClipsGroup
{
	public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
	public AudioClip[] audioClips;
	public Onomatopoeia Onomatopoeia = default;

	private int _nextClipToPlay = -1;
	private int _lastClipPlayed = -1;

	/// <summary>
	/// Chooses the next clip in the sequence, either following the order or randomly.
	/// </summary>
	/// <returns>A reference to an AudioClip</returns>
	public VisualisableAudioClip GetNextClip()
	{
		// Fast out if there is only one clip to play
		if (audioClips.Length == 1)
			return new VisualisableAudioClip(audioClips[0], Onomatopoeia);

		if (_nextClipToPlay == -1)
		{
			// Index needs to be initialised: 0 if Sequential, random if otherwise
			_nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
		}
		else
		{
			// Select next clip index based on the appropriate SequenceMode
			switch (sequenceMode)
			{
				case SequenceMode.Random:
					_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
					break;

				case SequenceMode.RandomNoImmediateRepeat:
					do
					{
						_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
					} while (_nextClipToPlay == _lastClipPlayed);
					break;

				case SequenceMode.Sequential:
					_nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
					break;
			}
		}

		_lastClipPlayed = _nextClipToPlay;

		return new VisualisableAudioClip(audioClips[_nextClipToPlay], Onomatopoeia);
	}

	public enum SequenceMode
	{
		Random,
		RandomNoImmediateRepeat,
		Sequential,
	}
}

[Serializable]
public class Onomatopoeia
{
	public bool Visualise = true;	
	public LocalizedString SoundText;
	public float Duration = 1f;
}
