using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[System.Serializable]
public class ActorExpressions
{
	public ActorSO Actor = default;
	public ActorMood CurrentMood = ActorMood.NONE;
	public MoodCollectionSO DefaultMoodSet = null;
	public List<MoodCollectionSO> MoodSets = new List<MoodCollectionSO>();
	public LocalizedAsset<PhonemeSetSO> LocalizedAsset = new LocalizedAsset<PhonemeSetSO>();
}

[System.Serializable]
public class ActorAnimationSettings
{
	public ActorSO Actor = default;
	public bool PlayRandomAnimation = true;
	public int ForcedAnimationIndex = -1;
}

public class ExpressionManagerMulti : MonoBehaviour
{
	// Inspector Assigned
	[SerializeField] private List<ActorExpressions> _actorExpressions = new List<ActorExpressions>();
	
	[Header("Idle Blink Settings (all actors)")]
	// Vector2 used to give min / max range that is used for random selection of float value
	[Tooltip("BlinkTimeRange - defines a min / max range for how many seconds should elapse before blinking")]
	[SerializeField] private Vector2 _blinkFrequencyRange = new Vector2(0.5f, 2.0f);
	[Tooltip("MidBlinkTimeRange - defines a min / max range for how many seconds should elapse with the eye in mid-blink position")]
	[SerializeField] private Vector2 _midBlinkDurationRange = new Vector2(0.1f, 0.35f);
	[Tooltip("EyeClosedTimeRange - defines a min / max range for how many seconds should elapse with the eye in closed position")]
	[SerializeField] private Vector2 _eyeClosedDurationRange = new Vector2(0.2f, 1.5f);
	[Tooltip("We want the character to blink less during dialogue...this controls the timing between blinks...eye closed duration and mid-blink duration do not need to be scaled")]
	[SerializeField] private float _dialogueBlinkFrequencyMultiplier = 2.0f;

	[Header("Phoneme Settings (all actors)")]
	[SerializeField] private float _maxMouthShapeDuration = 0.3f;
	
	// Hidden
	[HideInInspector] public bool playRandomAnimation = true;
	[HideInInspector] public int animationIndex = 0;

	// Dictionaries - for fast runtime lookup
	private List<ActorSO> _registeredActors = new List<ActorSO>();
	private Dictionary<ActorSO, ActorAnimationSettings> _actorAnimationSettingsDictionary = new Dictionary<ActorSO, ActorAnimationSettings>();
	private Dictionary<ActorSO, ActorExpressions> _actorExpressionDictionary = new Dictionary<ActorSO, ActorExpressions>();
	private Dictionary<ActorSO, Dictionary<ActorMood, MoodCollectionSO>> _moodDictionary = new Dictionary<ActorSO, Dictionary<ActorMood, MoodCollectionSO>>();
	private ActorSO _activeActor = null;
	private Dictionary<ActorSO, MoodCollectionSO> _activeMoodSet = new Dictionary<ActorSO, MoodCollectionSO>();
	private Dictionary<ActorSO, PhonemeSetSO> _activePhonemeSet = new Dictionary<ActorSO, PhonemeSetSO>();
	private Dictionary<ActorSO, float> _mouthTimer = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float> _blinkTimer = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float> _startBlinkTime = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float> _closingMidBlinkDuration = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float> _openingMidBlinkDuration = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float> _eyeClosedDuration = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, bool> _isCharacterTalking = new Dictionary<ActorSO, bool>();
	private bool _dictionariesBuilt = false;

	public void RegisterActor(ActorSO actor)
	{
		_registeredActors.Add(actor);
	}

	public void UnregisterActor(ActorSO actor)
	{
		_registeredActors.Remove(actor);
	}

	public void SetActorAnimationSettings(ActorSO actor, ActorAnimationSettings settings)
	{	ActorAnimationSettings s;
		if (!_actorAnimationSettingsDictionary.TryGetValue(actor, out s))
		{
			_actorAnimationSettingsDictionary.Add(actor, settings);
		}
		else
		{
			_actorAnimationSettingsDictionary[actor] = settings;
		}
	}

	public void ForceDefaultAnimation(ActorSO actor)
	{
		actor.TransitionToDefaultAnimClip();
	}

	public void SetCharacterTalkingState(ActorSO actor, bool talking)
	{
		bool value;
		if (_isCharacterTalking.TryGetValue(actor, out value))
		{
			_isCharacterTalking[actor] = talking;
		}
		else
		{
			_isCharacterTalking.Add(actor, talking);
		}
	}

	// This function updates the active mood and the phoneme set in use for the actor, based on their current mood
	public void SetPhonemeLibraryByMood(ActorSO actor, ActorMood mood)
	{
		// First, make sure our lookup dictionary is initialized!
		if (!_dictionariesBuilt)
			BuildDictionaries();

		// Get the corresponding MoodSet for our actor's new mood
		Dictionary<ActorMood, MoodCollectionSO> actorMoodDictionary = new Dictionary<ActorMood, MoodCollectionSO>();
		if (_moodDictionary.TryGetValue(actor, out actorMoodDictionary))
		{
			MoodCollectionSO moodSet = null;
			if (actorMoodDictionary.TryGetValue(mood, out moodSet))
			{
				// Update active mood set
				MoodCollectionSO tmp;
				if (_activeMoodSet.TryGetValue(actor, out tmp))
				{
					_activeMoodSet[actor] = moodSet;
				}
				else
				{
					_activeMoodSet.Add(actor, moodSet);
				}

				// Update the active PhonemeSet based on the mood
				if (moodSet.LocalizedPhonemeSet != null)
				{
					// The only localized part of the MoodSet is the PhonemeSet (i.e., the system responsbile for determining the mouth shape,
					// which is meant to match the dialogue text on-screen). This system uses Localization because the phonemes (mouth shape
					// texture) are language-specific.
					var AsyncAssetOp = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<PhonemeSetSO>(moodSet.LocalizedPhonemeSet.TableReference,
																											   moodSet.LocalizedPhonemeSet.TableEntryReference);

					// We can only get the localized asset (i.e., the underlying PhonemeSet) when the async operation is done
					if (AsyncAssetOp.IsDone)
					{
						// Get the localized asset
						PhonemeSetSO localizedPhonemeSet = AsyncAssetOp.Result;

						// Make the localized PhonemeSet the active one
						PhonemeSetSO phonemeSet;
						if (_activePhonemeSet.TryGetValue(actor, out phonemeSet))
						{
							_activePhonemeSet[actor] = localizedPhonemeSet;							
						}
						else
						{
							_activePhonemeSet.Add(actor, localizedPhonemeSet);
						}

						// Make sure to initialize the PhonemeSet so that its internal lookup dictionaries are built
						_activePhonemeSet[actor].Initialize();
					}
				}
			}
		}
		else
		{
			Debug.LogWarning("No mood set found for actor : " + actor);
		}
	}

	// This function takes in a phoneme key, which is meant to represent a base sound - like "CH", as in the word 'cheese.' If the key is
	// set to "." or null, that is interpreted as the silent space between two phonemes and the default mouth texture defined on the
	// ActorSO is used. 
	public void SetActivePhoneme(ActorSO actor, string phonemeKey)
	{
		if (phonemeKey == "." || _activePhonemeSet == null)
		{
			actor.SetDefaultMouthTexture();
		}
		else
		{
			// Set the appropriate mouth texture for this phoneme key
			actor.SetMouthTexture(_activePhonemeSet[actor].GetMouthShape(phonemeKey));

			// Reset this actor's mouth timer for this new mouth shape
			_mouthTimer[actor] = 0.0f;
		}
	}

	private void BuildDictionaries()
	{
		_actorExpressionDictionary.Clear();
		_moodDictionary.Clear();

		foreach (ActorExpressions expression in _actorExpressions)
		{
			_actorExpressionDictionary.Add(expression.Actor, expression);

			Dictionary<ActorMood, MoodCollectionSO> _tmp = new Dictionary<ActorMood, MoodCollectionSO>();
			foreach (MoodCollectionSO moodSet in expression.MoodSets)
			{
				_tmp.Add(moodSet.Mood, moodSet);
			}

			_moodDictionary.Add(expression.Actor, _tmp);
		}

		_dictionariesBuilt = true;
	}

	private void Start()
	{
		BuildDictionaries();
		// Sample new blink values
	}

	private void Update()
	{
		// Make sure lookup dictionaries have been built!
		if (_dictionariesBuilt)
			BuildDictionaries();

		// Loop over all actors registered
		foreach (ActorSO actor in _registeredActors)
		{
			#region (1) Set the Actor Mood
			// This system always requires a mood (happy / sad / etc.) to be set, so if no mood set is currently active, set to default
			// First, check if we have an entry for this actor in activeMoodSet dictionary
			MoodCollectionSO moodSet;
			if (!_activeMoodSet.TryGetValue(actor, out moodSet))
			{
				// If we don't, then we need to set the mood set to the default mood set
				// Start by tring to get the actor expression set
				ActorExpressions actorExpressions;
				if (_actorExpressionDictionary.TryGetValue(actor, out actorExpressions))
				{
					// If we found an entry, then we can add the default mood set
					moodSet = actorExpressions.DefaultMoodSet;
					_activeMoodSet.Add(actor, actorExpressions.DefaultMoodSet);
				}
				else
				{
					// If we didn't find an entry, then you messed up!
					Debug.LogError("Please add an ActorExpression for this ActorSO! : " + actor);
				}
			}
			#endregion

			#region (2) Configure Actor Timers
			// Blink Timer
			float blinkTimerValue;
			if (!_blinkTimer.TryGetValue(actor, out blinkTimerValue))
			{
				_blinkTimer.Add(actor, 0.0f);
			}
			else
			{
				_blinkTimer[actor] += Time.deltaTime;
			}

			// Mouth Timer
			float mouthTimerValue;
			if (!_mouthTimer.TryGetValue(actor, out mouthTimerValue))
			{
				_mouthTimer.Add(actor, 0.0f);
			}
			else
			{
				_mouthTimer[actor] += Time.deltaTime;
			}

			// Transition to default mouth texture if current mouth shape exceeds max duration
			if (_mouthTimer[actor] > _maxMouthShapeDuration)
			{
				if (actor != null)
					actor.SetDefaultMouthTexture();
			}
			#endregion

			#region (3) Handle Animation Pose Changes
			int clipIndex = 0;

			// Try getting the animation settings for this actor
			ActorAnimationSettings animSettings;
			if (_actorAnimationSettingsDictionary.TryGetValue(actor, out animSettings))
			{
				// Select the animation clip according to the settings
				if (animSettings.PlayRandomAnimation)
				{
					clipIndex = Mathf.RoundToInt(Random.Range(0, moodSet.AnimatorTitleHashes.Count - 1));
				}
				else
				{
					clipIndex = animSettings.ForcedAnimationIndex;
				}

				// Make sure it is a valid index
				if (clipIndex >= 0 && clipIndex <= moodSet.AnimatorClipTitles.Count - 1)
				{
					// Then get the hash
					int clipTitleHash = moodSet.AnimatorTitleHashes[clipIndex];

					// And transition to the new animation
					actor.TransitionToAnimatorClip(clipTitleHash, 0.0f);
				}

				// If no animations are stored in the mood set, just transition to the actor's default animation
				if (moodSet.AnimatorTitleHashes.Count == 0)
				{
					actor.TransitionToDefaultAnimClip();
				}
			}
			#endregion

			#region (4) Handle Blinking   
			// Time to blink
			if (_blinkTimer[actor] < _startBlinkTime[actor])
			{
				// Even though eyes are probably already open, set them open here just in case
				actor.SetEyeTexture(moodSet.EyeState_Open);
			}
			else if (_blinkTimer[actor] >= _startBlinkTime[actor] && _blinkTimer[actor] < _closingMidBlinkDuration[actor])
			{
				// Now transition to the mid-blink eye texture
				actor.SetEyeTexture(moodSet.EyeState_MidBlink);
			}
			else if (_blinkTimer[actor] >= _closingMidBlinkDuration[actor] && _blinkTimer[actor] < _eyeClosedDuration[actor])
			{
				// Now transition to the eye-closed texture
				actor.SetEyeTexture(moodSet.EyeState_Closed);
			}
			else if (_blinkTimer[actor] >= _eyeClosedDuration[actor] && _blinkTimer[actor] < _openingMidBlinkDuration[actor])
			{
				// On our way back up...transition to the mid-blink texture again
				actor.SetEyeTexture(moodSet.EyeState_MidBlink);
			}
			else if (_blinkTimer[actor] >= _openingMidBlinkDuration[actor])
			{
				// Finally, blink is done...set the eye to open texture, sample new random blink values, and
				// reset the blink timer
				actor.SetEyeTexture(moodSet.EyeState_Open);

				// Sample new blink values for the next blink sequence
				SampleNewBlinkValues(actor);

				// Reset the timer for the next blink sequence
				_blinkTimer[actor] = 0.0f;
			}
			#endregion			
		}
	}
	
	private void SampleNewBlinkValues(ActorSO actor)
	{
		// Randomly sample values for the blinking system
		bool talking;
		if (_isCharacterTalking.TryGetValue(actor, out talking))
		{
			// value is already set!
		}
		else
		{
			talking = false;
		}

		float currValue;
		float newValue = Random.Range(_blinkFrequencyRange.x, _blinkFrequencyRange.y) *(talking ? _dialogueBlinkFrequencyMultiplier : 1.0f);
		if (_startBlinkTime.TryGetValue(actor, out currValue))
		{
			_startBlinkTime[actor] = newValue;
		}
		else
		{
			_startBlinkTime.Add(actor, newValue);
		}

		newValue = Random.Range(_midBlinkDurationRange.x, _midBlinkDurationRange.y);
		if (_closingMidBlinkDuration.TryGetValue(actor, out currValue))
		{
			_closingMidBlinkDuration[actor] = newValue;
		}
		else
		{
			_closingMidBlinkDuration.Add(actor, newValue);
		}

		newValue = Random.Range(_eyeClosedDurationRange.x, _eyeClosedDurationRange.y);
		if (_eyeClosedDuration.TryGetValue(actor, out currValue))
		{
			_eyeClosedDuration[actor] = newValue;
		}
		else
		{
			_eyeClosedDuration.Add(actor, newValue);
		}

		if (_openingMidBlinkDuration.TryGetValue(actor, out currValue))
		{
			_openingMidBlinkDuration[actor] = _closingMidBlinkDuration[actor];
		}
		else
		{
			_openingMidBlinkDuration.Add(actor, _closingMidBlinkDuration[actor]);
		}

		// Make all of these times cumulative
		_closingMidBlinkDuration[actor] += _startBlinkTime[actor];
		_eyeClosedDuration[actor] += _closingMidBlinkDuration[actor];
		_openingMidBlinkDuration[actor] += _eyeClosedDuration[actor];
	}
}
