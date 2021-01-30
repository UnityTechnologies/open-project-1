using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

// NOTE TO PR-MERGER: I have marked a few sections with the region tags "DELETABLE_DEMO_CODE"...this was code specifically used for the demo that is not
// critical to the operation of the Expression System itself...and, as its name suggests, it can be deleted without any loss of core functionality

// This class gives the Expression Manager an understanding of what moods and phonemes are available for actors. You MUST define an ActorExpression (on the
// Inspector of a MonoBehaviour object with this script as a component) if you want to use the Expression System to apply expressions to that actor!
// If you fail to do this, you will receive a Debug.LogError() reminding you to appropriately set up expressions for the actors in the scene.
[System.Serializable]
public class ActorExpressions
{
	public ActorSO						Actor		   = default;
	public ActorMood					CurrentMood    = ActorMood.NONE;
	public MoodCollectionSO				DefaultMoodSet = null;
	public List<MoodCollectionSO>		MoodSets       = new List<MoodCollectionSO>();
	public LocalizedAsset<PhonemeSetSO> LocalizedAsset = new LocalizedAsset<PhonemeSetSO>();
}

// This is just a helper class for indicating how to select from the array of animations in a MoodCollectionSO.
[System.Serializable]
public class ActorAnimationSettings
{
	public ActorSO Actor                = default;
	public bool    PlayRandomAnimation  = true;
	public int     ForcedAnimationIndex = -1;
}

public class ExpressionManager : MonoBehaviour
{
	// Inspector Assigned
	[SerializeField] private List<ActorExpressions> _actorExpressions = new List<ActorExpressions>();
	
	[Header("Idle Blink Settings (all actors)")]
	// Vector2 used to give min / max range that is used for random selection of float value
	[Tooltip("BlinkTimeRange - defines a min / max range for how many seconds should elapse before blinking")]
	[SerializeField] private Vector2 _blinkFrequencyRange              = new Vector2(0.5f, 2.0f);
	[Tooltip("MidBlinkTimeRange - defines a min / max range for how many seconds should elapse with the eye in mid-blink position")]
	[SerializeField] private Vector2 _midBlinkDurationRange            = new Vector2(0.1f, 0.35f);
	[Tooltip("EyeClosedTimeRange - defines a min / max range for how many seconds should elapse with the eye in closed position")]
	[SerializeField] private Vector2 _eyeClosedDurationRange           = new Vector2(0.2f, 1.5f);
	[Tooltip("We want the character to blink less during dialogue...this controls the timing between blinks...eye closed duration and mid-blink duration do not need to be scaled")]
	[SerializeField] private float   _dialogueBlinkFrequencyMultiplier = 2.0f;

	[Header("Phoneme Settings (all actors)")]
	[SerializeField] private float _maxMouthShapeDuration = 0.3f;

	#region DELETABLE_DEMO_CODE
	[Header("Temporary UI")]
	[SerializeField] private TextMeshProUGUI _goodHamletMoodText;
	[SerializeField] private TextMeshProUGUI _evilHamletMoodText;
	[SerializeField] private TextMeshProUGUI _townsfolkMoodText;
	#endregion

	// Hidden
	[HideInInspector] public bool playRandomAnimation = true;
	[HideInInspector] public int  animationIndex      = 0;

	// Only actors in this list can be controlled by ExpressionManager
	private List<ActorSO> _registeredActors = new List<ActorSO>();

	// Dictionaries - for fast runtime lookup
	private Dictionary<ActorSO, Dictionary<ActorMood, MoodCollectionSO>> _moodDictionary  = new Dictionary<ActorSO, Dictionary<ActorMood, MoodCollectionSO>>();
	private Dictionary<ActorSO, ActorAnimationSettings> _actorAnimationSettingsDictionary = new Dictionary<ActorSO, ActorAnimationSettings>();
	private Dictionary<ActorSO, ActorExpressions>       _actorExpressionDictionary        = new Dictionary<ActorSO, ActorExpressions>();
	private Dictionary<ActorSO, MoodCollectionSO>       _activeMoodSet                    = new Dictionary<ActorSO, MoodCollectionSO>();
	private Dictionary<ActorSO, MoodCollectionSO>       _lastMoodSet                      = new Dictionary<ActorSO, MoodCollectionSO>();
	private Dictionary<ActorSO, PhonemeSetSO>           _activePhonemeSet                 = new Dictionary<ActorSO, PhonemeSetSO>();
	private Dictionary<ActorSO, List<BlendTarget>>      _mouthBlendTargets                = new Dictionary<ActorSO, List<BlendTarget>>();
	private Dictionary<ActorSO, float>                  _mouthTimer                       = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>                  _blinkTimer                       = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>                  _eye3DTimer                       = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>				    _mouth3DTimer                     = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>				    _startBlinkTime                   = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>					_closingMidBlinkDuration          = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>					_openingMidBlinkDuration          = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, float>					_eyeClosedDuration                = new Dictionary<ActorSO, float>();
	private Dictionary<ActorSO, bool>					_isCharacterTalking               = new Dictionary<ActorSO, bool>();
	private Dictionary<ActorSO, ActorEyeType>			_enableBlinking                   = new Dictionary<ActorSO, ActorEyeType>();
	private Dictionary<ActorSO, ActorMouthType>			_enablePhonemes                   = new Dictionary<ActorSO, ActorMouthType>();
	private Dictionary<ActorSO, bool>					_enableAnimations                 = new Dictionary<ActorSO, bool>();
	private bool _dictionariesBuilt = false;

	// METHODS

	#region MAIN FUNCTIONS

	// Builds the main dictionaries needed for this system to work
	private void BuildDictionaries()
	{
		// Start by clearing dictionaries
		_actorExpressionDictionary.Clear();
		_moodDictionary.Clear();

		// All actors must have ActorExpressions set in order to use ExpressionManager on them!
		if (_actorExpressions == null)
		{
			Debug.LogError("You must define expressions for all actors that you intend to use this system for!");
		}

		// Build the dictionaries
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

	// Call the BuildDictionaries function at Start
	private void Start()
	{
		BuildDictionaries();
	}

	private void Update()
	{
		// Make sure lookup dictionaries have been built!
		if (_dictionariesBuilt)
			BuildDictionaries();

		// No point in continuing if we don't have any actors registered to this manager!
		if (_registeredActors == null)
			return;

		// Loop over all registered actors
		foreach (ActorSO actor in _registeredActors)
		{
			#region (1) Set the Actor Mood
			// This system always requires a mood (happy / sad / etc.) to be set, so if no mood set is currently active, set to default
			// First, check if we have an entry for this actor in activeMoodSet dictionary
			MoodCollectionSO moodSet = null;
			if (_activeMoodSet == null || !_activeMoodSet.TryGetValue(actor, out moodSet))
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

			// If for some reason we still don't have an active mood set, we can't proceed
			if (moodSet == null)
				return;

			#endregion

			#region (2) Configure Actor Timers

			if (TypeOfBlinkingEnabled(actor) != ActorEyeType.Undefined)
			{
				// Blink Timer
				float blinkTimerValue;
				if (_blinkTimer == null || !_blinkTimer.TryGetValue(actor, out blinkTimerValue))
				{
					Debug.Log("Resetting blink timer due to null");
					_blinkTimer.Add(actor, 0.0f);
				}
				else
				{
					_blinkTimer[actor] += Time.deltaTime;
				}
			}

			if (TypeOfBlinkingEnabled(actor) == ActorEyeType.ThreeD)
			{
				// Eye Timer
				float eyeTimeValue;
				if (_eye3DTimer == null || !_eye3DTimer.TryGetValue(actor, out eyeTimeValue) || _lastMoodSet[actor] == null)
				{
					Debug.Log("Resetting eye timer due to null");
					_eye3DTimer.Add(actor, 0.0f);
				}
				else
				{
					_eye3DTimer[actor] += Time.deltaTime;
				}
			}

			if (TypeOfPhonemesEnabled(actor) == ActorMouthType.ThreeD)
			{
				// Mouth Timer
				float mouthTimeValue;
				if (_mouth3DTimer == null || !_mouth3DTimer.TryGetValue(actor, out mouthTimeValue) || _lastMoodSet[actor] == null)
				{
					Debug.Log("Resetting mouth 3D timer due to null");
					_mouth3DTimer.Add(actor, 0.0f);
				}
				else
				{
					_mouth3DTimer[actor] += Time.deltaTime;
				}
			}

			if (TypeOfPhonemesEnabled(actor) != ActorMouthType.Undefined)
			{
				// Mouth Timer
				float mouthTimerValue;
				if (_mouthTimer == null || !_mouthTimer.TryGetValue(actor, out mouthTimerValue))
				{
					Debug.Log("Resetting mouth timer due to null");
					_mouthTimer.Add(actor, 0.0f);
				}
				else
				{
					_mouthTimer[actor] += Time.deltaTime;
				}

				// Transition to default mouth texture if current mouth shape exceeds max duration
				if (_mouthTimer[actor] > _maxMouthShapeDuration)
				{
					if (TypeOfPhonemesEnabled(actor) == ActorMouthType.TwoD)
					{
						if (actor != null)
							actor.SetDefaultMouthTexture();
					}
					else
					{
						if (actor != null)
							actor.SetDefaultMouthBlendShape();
					}
				}
			}
			#endregion

			#region (3) Handle Blinking
			if (TypeOfBlinkingEnabled(actor) != ActorEyeType.Undefined)
			{
				// First, check dictionary values to see if blink values have been populated
				float value;
				if (!_startBlinkTime.TryGetValue(actor, out value) ||
					!_closingMidBlinkDuration.TryGetValue(actor, out value) ||
					!_eyeClosedDuration.TryGetValue(actor, out value) ||
					!_openingMidBlinkDuration.TryGetValue(actor, out value))
				{
					SampleNewBlinkValues(actor);
				}

				// 2D (texture) blinking
				if (TypeOfBlinkingEnabled(actor) == ActorEyeType.TwoD)
				{
					// Time to blink
					if (_blinkTimer[actor] < _startBlinkTime[actor])
					{
						// Even though eyes are probably already open, set them open here just in case
						actor.SetEyeTexture(moodSet.EyeState_Open_2D);			
					}
					else if (_blinkTimer[actor] >= _startBlinkTime[actor] && _blinkTimer[actor] < _closingMidBlinkDuration[actor])
					{
						// Now transition to the mid-blink eye texture
						actor.SetEyeTexture(moodSet.EyeState_MidBlink_2D);
					}
					else if (_blinkTimer[actor] >= _closingMidBlinkDuration[actor] && _blinkTimer[actor] < _eyeClosedDuration[actor])
					{
						// Now transition to the eye-closed texture
						actor.SetEyeTexture(moodSet.EyeState_Closed_2D);
					}
					else if (_blinkTimer[actor] >= _eyeClosedDuration[actor] && _blinkTimer[actor] < _openingMidBlinkDuration[actor])
					{
						// On our way back up...transition to the mid-blink texture again
						actor.SetEyeTexture(moodSet.EyeState_MidBlink_2D);
					}
					else if (_blinkTimer[actor] >= _openingMidBlinkDuration[actor])
					{
						// Finally, blink is done...set the eye to open texture, sample new random blink values, and
						// reset the blink timer
						actor.SetEyeTexture(moodSet.EyeState_Open_2D);

						// Sample new blink values for the next blink sequence
						SampleNewBlinkValues(actor);

						// Reset the timer for the next blink sequence
						_blinkTimer[actor] = 0.0f;
					}
				}

				// 3D (blendshape) blinking
				if (TypeOfBlinkingEnabled(actor) == ActorEyeType.ThreeD)
				{
					float T = 0.0f;
					if (_blinkTimer[actor] >= _startBlinkTime[actor] && _blinkTimer[actor] < _eyeClosedDuration[actor])
					{
						T = 100.0f * (_blinkTimer[actor] - _startBlinkTime[actor]) / (_eyeClosedDuration[actor] - _startBlinkTime[actor]);
					}

					if (_blinkTimer[actor] >= _eyeClosedDuration[actor] && _blinkTimer[actor] < _openingMidBlinkDuration[actor])
					{
						T = 100.0f * (1 - (_blinkTimer[actor] - _eyeClosedDuration[actor]) / (_openingMidBlinkDuration[actor] - _eyeClosedDuration[actor]));
					}
										
					if (_blinkTimer[actor] >= _openingMidBlinkDuration[actor])
					{
						T = 0.0f;

						SampleNewBlinkValues(actor);

						_blinkTimer[actor] = 0.0f;
					}

					actor.SetBlendTarget(moodSet.Blink_3D, T);
				}
			}
			#endregion

			// Mood blendshapes
			#region (4) Handle 3D Mood Blend Targets for Eyes and Mouth

			if (TypeOfBlinkingEnabled(actor) == ActorEyeType.ThreeD)
			{
				foreach (BlendTarget bt in moodSet.EyeMoodTargets)
				{
					float T = Mathf.Clamp01(_eye3DTimer[actor] / moodSet.MoodTransitionTime);

					float w = bt.BlendWeight * T;

					// Takes care of returning previously set blend values back to zero
					if (bt.BlendWeight == 0.0f && actor.GetBlendWeight(bt.BlendShape) > 0.5f)
					{
						w = 100.0f * (1 - T);
					}

					actor.SetBlendTarget(bt.BlendShape, w);

					if (_eye3DTimer[actor] >= moodSet.MoodTransitionTime && moodSet != _lastMoodSet[actor])
						_eye3DTimer[actor] = 0.0f;
				}
			}

			if (TypeOfPhonemesEnabled(actor) == ActorMouthType.ThreeD)
			{
				foreach (BlendTarget bt in moodSet.MouthMoodTargets)
				{
					float T = Mathf.Clamp01(_mouth3DTimer[actor] / moodSet.MoodTransitionTime);

					float w = bt.BlendWeight * T;

					// Takes care of returning previously set blend values back to zero
					if (bt.BlendWeight == 0.0f && actor.GetBlendWeight(bt.BlendShape) > 0.5f)
					{
						w = 100.0f *  (1 - T);
					}

					actor.SetBlendTarget(bt.BlendShape, w);

					if (_mouth3DTimer[actor] >= moodSet.MoodTransitionTime && moodSet != _lastMoodSet[actor])
						_mouth3DTimer[actor] = 0.0f;
				}
			}

			#endregion
			
			// 3D (blendshape) phonemes
			#region (5) Handle 3D Blend Targets for Mouths

			if (TypeOfPhonemesEnabled(actor) == ActorMouthType.ThreeD)
			{
				List<BlendTarget> list = null;
				if (_mouthBlendTargets.TryGetValue(actor, out list))
				{
					if (list != null)
					{
						foreach(BlendTarget bt in list)
						{
							float T = Mathf.Clamp01(_mouthTimer[actor] / _maxMouthShapeDuration);

							float w = bt.BlendWeight * T;
							float goal = 0.99f * bt.BlendWeight;

							if (bt.BlendWeight == 0.0f && actor.GetBlendWeight(bt.BlendShape) > 0.5f)
							{
								w = 100.0f * (1 - T);

								goal = 0.01f;
							}

							actor.SetBlendTarget(bt.BlendShape, w);
							
							if (goal == 0.01f)
							{
								if (w <= goal)
								{
									actor.SetBlendTarget(bt.BlendShape, 0.0f);
								}
							}
							else
							{
								if (w >= goal)
								{
									actor.SetBlendTarget(bt.BlendShape, 0.0f);
								}
							}
						}
					}
					else
					{
						actor.SetDefaultMouthBlendShape();
					}				
				}
				else
				{
					actor.SetDefaultMouthBlendShape();
				}
			}

			#endregion
	
			// Record last mood so that it can be compared next cycle
			_lastMoodSet[actor] = moodSet;
		}
	}

	#endregion

	#region Enable / Check Functions

	public void EnableBlinking(ActorSO actor, ActorEyeType value)
	{
		ActorEyeType currValue;
		if (!_enableBlinking.TryGetValue(actor, out currValue))
			_enableBlinking.Add(actor, value);
		else
			_enableBlinking[actor] = value;
	}

	public void DisableBlinking(ActorSO actor)
	{
		ActorEyeType currValue;
		if (_enableBlinking.TryGetValue(actor, out currValue))
			_enableBlinking[actor] = ActorEyeType.Undefined;
		else
			_enableBlinking.Add(actor, ActorEyeType.Undefined);
	}

	public void EnablePhonemes(ActorSO actor, ActorMouthType value)
	{
		ActorMouthType currValue;
		if (!_enablePhonemes.TryGetValue(actor, out currValue))
			_enablePhonemes.Add(actor, value);
		else
			_enablePhonemes[actor] = value;
	}

	public void DisablePhonemes(ActorSO actor)
	{
		ActorMouthType currValue;
		if (_enablePhonemes.TryGetValue(actor, out currValue))
			_enablePhonemes[actor] = ActorMouthType.Undefined;
		else
			_enablePhonemes.Add(actor, ActorMouthType.Undefined);
	}

	public void EnableAnimations(ActorSO actor, bool value)
	{
		bool currValue;
		if (!_enableAnimations.TryGetValue(actor, out currValue))
			_enableAnimations.Add(actor, value);
		else
			_enableAnimations[actor] = value;
	}

	private ActorEyeType TypeOfBlinkingEnabled(ActorSO actor)
	{
		ActorEyeType enableBlinking = ActorEyeType.Undefined;
		if (_enableBlinking.TryGetValue(actor, out enableBlinking))
		{
			// do nothing	
		}

		return enableBlinking;
	}

	private ActorMouthType TypeOfPhonemesEnabled(ActorSO actor)
	{
		ActorMouthType enablePhonemes = ActorMouthType.Undefined;
		if (_enablePhonemes.TryGetValue(actor, out enablePhonemes))
		{
			// do nothing
		}

		return enablePhonemes;
	}

	private bool AreAnimationsEnabled(ActorSO actor)
	{
		bool enableAnimations = true;
		if (_enableAnimations.TryGetValue(actor, out enableAnimations))
		{
			// do nothing
		}

		return enableAnimations;
	}

	#endregion

	#region Actor Functions

	// Register a new actor to the Expression Manager
	public void RegisterActor(ActorSO actor)
	{
		_registeredActors.Add(actor);

		// Make sure blink and mouth timers exist and are reset
		float value;
		if (!_blinkTimer.TryGetValue(actor, out value))
			_blinkTimer.Add(actor, 0.0f);

		if (!_mouthTimer.TryGetValue(actor, out value))
			_mouthTimer.Add(actor, 0.0f);

		SampleNewBlinkValues(actor);

		// Call actor.OnStart() so that material and blendshape properties are backed up
		actor.OnStart();
	}

	// Unregister an actor from the Expression manager
	public void UnregisterActor(ActorSO actor)
	{
		_registeredActors.Remove(actor);
	}
	
	private void OnDestroy()
	{
		// Make sure material properties revert to edit mode settings
		foreach (ActorSO actor in _registeredActors)
			actor.OnReset();
	}

	// Check to see if a particular actor is registered (and therefore, able to have expressions applied)
	public bool IsActorRegistered(ActorSO actor)
	{
		return _registeredActors.Contains(actor);
	}

	#endregion

	#region Blinking Functions

	private void SampleNewBlinkValues(ActorSO actor)
	{
		if (TypeOfBlinkingEnabled(actor) == ActorEyeType.Undefined)
			return;

		// Randomly sample values for the blinking system
		bool talking;
		if (_isCharacterTalking.TryGetValue(actor, out talking))
		{
			talking = true;
		}
		else
		{
			talking = false;
		}

		float currValue;
		float newValue = Random.Range(_blinkFrequencyRange.x, _blinkFrequencyRange.y) * (talking ? _dialogueBlinkFrequencyMultiplier : 1.0f);
		if (_startBlinkTime != null && _startBlinkTime.TryGetValue(actor, out currValue))
		{
			_startBlinkTime[actor] = newValue;
		}
		else
		{
			_startBlinkTime.Add(actor, newValue);
		}

		newValue = Random.Range(_midBlinkDurationRange.x, _midBlinkDurationRange.y);
		if (_closingMidBlinkDuration != null && _closingMidBlinkDuration.TryGetValue(actor, out currValue))
		{
			_closingMidBlinkDuration[actor] = newValue;
		}
		else
		{
			_closingMidBlinkDuration.Add(actor, newValue);
		}

		newValue = Random.Range(_eyeClosedDurationRange.x, _eyeClosedDurationRange.y);
		if (_eyeClosedDuration != null && _eyeClosedDuration.TryGetValue(actor, out currValue))
		{
			_eyeClosedDuration[actor] = newValue;
		}
		else
		{
			_eyeClosedDuration.Add(actor, newValue);
		}

		if (_openingMidBlinkDuration != null && _openingMidBlinkDuration.TryGetValue(actor, out currValue))
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

		_blinkTimer[actor] = 0.0f;
	}

	#endregion

	#region Phoneme / Mouth Functions

	// This function updates the active mood, the phoneme set, and sets the animation in use for the actor, based on their current mood.
	// Work for both 2D and 3D characters
	public void SetPhonemeLibraryByMood(ActorSO actor, ActorMood mood)
	{
		// First, make sure our lookup dictionary is initialized!
		if (!_dictionariesBuilt)
			BuildDictionaries();

		#region DELETABLE_DEMO_CODE
		string moodColor = "white";
		switch (mood)
		{
			case ActorMood.HAPPY:
				moodColor = "blue";
				break;

			case ActorMood.EXPLAINING:
				moodColor = "green";
				break;

			case ActorMood.NEUTRAL:
				moodColor = "white";
				break;

			case ActorMood.SAD:
				moodColor = "black";
				break;

			case ActorMood.SCARED:
				moodColor = "yellow";
				break;

			case ActorMood.ANGRY:
				moodColor = "red";
				break;
		}

		// Extract the localized string
		var stringOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(actor.ActorName.TableReference, actor.ActorName.TableEntryReference);

		string actorName = null;
		actorName = stringOp.Result;
		if (stringOp.IsDone)
		{
			actorName = stringOp.Result;
		}
		else
		{
			stringOp.Completed += (op) => actorName = op.Result;
		}

		if (actorName == "Hamlet")
		{ 
			_goodHamletMoodText.text = "<color=green>Hamlet's <color=white>Mood:\n<size=150%><color=" + moodColor + ">" + mood.ToString().ToUpper();
		}

		if (actorName == "Evil Hamlet")
		{ 
			_evilHamletMoodText.text = "<color=red>Evil Hamlet's <color=white>Mood:\n<size=150%><color=" + moodColor + ">" + mood.ToString().ToUpper();
		}

		if (actorName == "Townsfolk")
		{
			_townsfolkMoodText.text = "<color=orange>Townsfolk_M's <color=white>Mood:\n<size=150%><color=" + moodColor + ">" + mood.ToString().ToUpper();
		}

		#endregion

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
				
				if (TypeOfPhonemesEnabled(actor) != ActorMouthType.Undefined)
				{
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

			if (AreAnimationsEnabled(actor))
			{
				int clipIndex = 0;

				// Now deal with the animation...
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
			}
		}
		else
		{
			Debug.LogWarning("No mood set found for actor : " + actor);
		}
	}

	// This function takes in a phoneme key, which is meant to represent a base sound - like "CH", as in the word 'cheese.' If the key is
	// set to "." or null, that is interpreted as the silent space between two phonemes and the default mouth texture defined on the
	// ActorSO is used. Also passes along the final call to the ActorSO object to set the appropriate mouth texture.
	// Works for 2D and 3D characters
	public void SetActivePhoneme(ActorSO actor, string phonemeKey)
	{
		if (!_registeredActors.Contains(actor))
		{
			Debug.LogError("You are trying to set a mouth texture on an unregistered actor!");
			return;
		}

		if (TypeOfPhonemesEnabled(actor) == ActorMouthType.Undefined)
			return;

		PhonemeSetSO phonemeSet;
		if (!_activePhonemeSet.TryGetValue(actor, out phonemeSet))
		{
			Debug.LogError("No phoneme set has been set for this actor!");
			return;
		}

		if (TypeOfPhonemesEnabled(actor) != ActorMouthType.Undefined)
		{
			float value;
			if (!_mouthTimer.TryGetValue(actor, out value))
			{
				Debug.LogError("No mouth timer present on this actor!");
				return;
			}
		}

		if (TypeOfPhonemesEnabled(actor) == ActorMouthType.TwoD)
		{
			if (phonemeKey == "." || _activePhonemeSet == null)
			{
					actor.SetDefaultMouthTexture();
			}
			else
			{
				// Set the appropriate mouth texture for this phoneme key
				actor.SetMouthTexture(_activePhonemeSet[actor].GetMouthShape(phonemeKey) as Texture2D);

				// Reset this actor's mouth timer for this new mouth shape
				_mouthTimer[actor] = 0.0f;
			}
		}

		if (TypeOfPhonemesEnabled(actor) == ActorMouthType.ThreeD)
		{
			if (phonemeKey == "." || _activePhonemeSet == null)
			{
				actor.SetDefaultMouthBlendShape();
			}
			else
			{
				List<BlendTarget> blendTargets = _activePhonemeSet[actor].GetMouthShape(phonemeKey) as List<BlendTarget>;

				// Clear any previous blend targets
				List<BlendTarget> list;
				if (_mouthBlendTargets.TryGetValue(actor, out list))
					_mouthBlendTargets.Clear();

				// And add new ones
				_mouthBlendTargets.Add(actor, blendTargets);
				_mouthTimer[actor] = 0.0f;
			}
		}
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

	// Just checks that a phoneme set exists for the given actor
	// Works for 2D and 3D characters
	public bool IsThereAnActivePhonemeSetForActor(ActorSO actor)
	{
		bool value;
		PhonemeSetSO phonemeSet;
		if (_activePhonemeSet.TryGetValue(actor, out phonemeSet))
		{
			return true;
		}

		return false;
	}

	#endregion

	#region Animation Functions

	// Set how we'd like animations to be picked from the MoodCollectionSO AnimationClipTitle array
	public void SetActorAnimationSettings(ActorSO actor, ActorAnimationSettings settings)
	{
		ActorAnimationSettings s;
		if (!_actorAnimationSettingsDictionary.TryGetValue(actor, out s))
		{
			_actorAnimationSettingsDictionary.Add(actor, settings);
		}
		else
		{
			_actorAnimationSettingsDictionary[actor] = settings;
		}
	}

	// This plays the default animation on the ActorSO object...useful for when the system either has no idea what clip
	// to play, or you just want the character to idle about (you can force this by unchecking "Play Random Animation"
	// and setting "Animation Index" = -1 (or any index < 0) on the CharacterMoodClip.
	public void ForceDefaultAnimation(ActorSO actor)
	{
		actor.TransitionToDefaultAnimClip();
	}

	#endregion
}
