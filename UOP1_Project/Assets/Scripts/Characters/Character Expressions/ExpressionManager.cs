using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

// TODO: Handle multiple actors???
public class ExpressionManager : MonoBehaviour
{
	[Header("Moods and Phonemes")]
	[SerializeField] private ActorMood _currentMood = ActorMood.NONE;
	[SerializeField] private MoodCollectionSO _defaultMoodSet = null;
	[SerializeField] private List<MoodCollectionSO> _moodSets = new List<MoodCollectionSO>();
	[SerializeField] private LocalizedAsset<PhonemeSetSO> _localizedAsset = new LocalizedAsset<PhonemeSetSO>();

	public TextMeshProUGUI moodText = null;

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

	// Private
	private Dictionary<ActorMood, MoodCollectionSO> _moodDictionary = new Dictionary<ActorMood, MoodCollectionSO>(); // for fast runtime lookup
	private ActorSO _activeActor = null;
	private MoodCollectionSO _activeMoodSet = null;
	private PhonemeSetSO _activePhonemeSet = null;
	private float _mouthTimer = 0.0f;
	private float _blinkTimer = 0.0f;
	private bool _moodDictionaryBuilt = false;
	private float _startBlinkTime = 0.0f;
	private float _closingMidBlinkDuration = 0.0f;
	private float _eyeClosedDuration = 0.0f;
	private float _openingMidBlinkDuration = 0.0f;
	private float _stopBlinkTime = 0.0f;

	// Public Accessors
	public ActorSO ActiveActor { get => _activeActor; set => _activeActor = value; }
	public MoodCollectionSO ActiveMood { get => _activeMoodSet; set => _activeMoodSet = value; }
	public PhonemeSetSO ActivePhonemeSet { get => _activePhonemeSet; set => _activePhonemeSet = value; }

	// Debug
	[Header("Debug")]
	public bool IsCharacterTalking = false;
	
	private void Start()
	{
		SampleNewBlinkValues();
	}

	private void Update()
	{
		// Make sure mood dictionary is initialized!
		if (!_moodDictionaryBuilt)
			BuildMoodDictionary();

		// This system always requires a mood (happy / sad / etc.) to be set, so if no mood set is currently active, set to default
		if (_activeMoodSet == null)
			_activeMoodSet = _defaultMoodSet;

		if (_activeActor == null)
			_activeActor = _activeMoodSet.Actor;

		if (_activeActor != null && _activeMoodSet != null)
		{
			// Handle blinking
			_blinkTimer += Time.deltaTime;

			// Time to blink
			if (_blinkTimer < _startBlinkTime)
			{ 
				// Even though eyes are probably already open, set them open here just in case
				_activeActor.SetEyeTexture(_activeMoodSet.EyeState_Open);
			}
			else if (_blinkTimer >= _startBlinkTime && _blinkTimer < _closingMidBlinkDuration)
			{
				// Now transition to the mid-blink eye texture
				_activeActor.SetEyeTexture(_activeMoodSet.EyeState_MidBlink);
			}
			else if (_blinkTimer >= _closingMidBlinkDuration && _blinkTimer < _eyeClosedDuration)
			{
				// Now transition to the eye-closed texture
				_activeActor.SetEyeTexture(_activeMoodSet.EyeState_Closed);
			}
			else if (_blinkTimer >= _eyeClosedDuration && _blinkTimer < _openingMidBlinkDuration)
			{
				// On our way back up...transition to the mid-blink texture again
				_activeActor.SetEyeTexture(_activeMoodSet.EyeState_MidBlink);
			}
			else if (_blinkTimer >= _openingMidBlinkDuration)
			{
				// Finally, blink is done...set the eye to open texture, sample new random blink values, and
				// reset the blink timer
				_activeActor.SetEyeTexture(_activeMoodSet.EyeState_Open);
				SampleNewBlinkValues();
				_blinkTimer = 0.0f;
			}
		}

		// Handle mouth shapes
		// Don't want to hold mouth shapes for too long, so just transition to default mouth shape if _maxMouthDuration is exceeded
		_mouthTimer += Time.deltaTime;

		if (_mouthTimer > _maxMouthShapeDuration)
		{
			if (_activeActor != null)
				_activeActor.SetDefaultMouthTexture();
		}
	}
	
	// Build a mood dictionary for fast runtime lookup
	private void BuildMoodDictionary()
	{
		_moodDictionary.Clear();

		foreach (MoodCollectionSO moodSet in _moodSets)
		{
			_moodDictionary.Add(moodSet.Mood, moodSet);
		}

		_moodDictionaryBuilt = true;
	}

	private void SampleNewBlinkValues()
	{
		// Randomly sample values for the blinking system
		_startBlinkTime = Random.Range(_blinkFrequencyRange.x, _blinkFrequencyRange.y) * (IsCharacterTalking ? _dialogueBlinkFrequencyMultiplier : 1.0f);
		_closingMidBlinkDuration = Random.Range(_midBlinkDurationRange.x, _midBlinkDurationRange.y);
		_eyeClosedDuration = Random.Range(_eyeClosedDurationRange.x, _eyeClosedDurationRange.y);
		_openingMidBlinkDuration = _closingMidBlinkDuration;

		// Make all of these times cumulative
		_closingMidBlinkDuration += _startBlinkTime;
		_eyeClosedDuration += _closingMidBlinkDuration;
		_openingMidBlinkDuration += _eyeClosedDuration;
	}

	public void PlayDefaultAnimClip()
	{
		_activeActor.TransitionToDefaultAnimClip();
	}

	public void SetMood(ActorMood mood)
	{
		// First, make sure our lookup dictionary is initialized!
		if (!_moodDictionaryBuilt)
			BuildMoodDictionary();

		_currentMood = mood;

		string moodColor = "white";
		switch(mood)
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

		moodText.text = "Current Mood: <size=150%><color=" + moodColor + ">" + mood.ToString().ToUpper();

		// Get the corresponding MoodSet for our actor's new mood
		MoodCollectionSO moodSet = null;
		if (_moodDictionary.TryGetValue(mood, out moodSet))
		{
			// Update active mood set
			_activeMoodSet = moodSet;

			// Let Update() handle eye textures since they'll be blinking over time!

			// Update the active PhonemeSet based on the new mood
			if (moodSet.LocalizedPhonemeSet != null)
			{
				// The only localized part of the MoodSet is the PhonemeSet (i.e., the system responsbile for determining the mouth shape,
				// which is meant to match the dialogue text on-screen). This system uses Localization because the phonemes (mouth shape
				// texture) are language-specific.
				var AsyncAssetOp = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<PhonemeSetSO>(_activeMoodSet.LocalizedPhonemeSet.TableReference,
																										 _activeMoodSet.LocalizedPhonemeSet.TableEntryReference);

				// We can only get the localized asset (i.e., the underlying PhonemeSet) when the async operation is done
				if (AsyncAssetOp.IsDone)
				{
					// Get the localized asset
					PhonemeSetSO localizedPhonemeSet = AsyncAssetOp.Result;

					// Build the phoneme dictionary using the PhonemeSet's Initialize() function
					_activePhonemeSet = localizedPhonemeSet;
					_activePhonemeSet.Initialize();
				}
			}
		}


		if (_activeMoodSet != null)
		{ 
			// Handle animation pose changes
			int clipIndex = 0;

			if (playRandomAnimation)
				clipIndex = Mathf.RoundToInt(Random.Range(0, _activeMoodSet.AnimatorTitleHashes.Count - 1));
			else
				clipIndex = animationIndex;

			if (_activeActor == null)
				_activeActor = _activeMoodSet.Actor;

			if (clipIndex <= _activeMoodSet.AnimatorTitleHashes.Count - 1 && clipIndex >= 0)
			{
				int clipTitle = _activeMoodSet.AnimatorTitleHashes[clipIndex];

				_activeActor.TransitionToAnimatorClip(clipTitle, 0.0f);
			}
			else
			{
				_activeActor.TransitionToDefaultAnimClip();
			}

			if (_activeMoodSet.AnimatorTitleHashes.Count == 0)
			{
				_activeActor.TransitionToDefaultAnimClip();
			}
		}
	}

	// This function takes in a phoneme key, which is meant to represent a base sound - like "CH", as in the word 'cheese.' If the key is
	// set to ".", that is interpreted as the silent space between two phonemes and the default mouth texture is used. 
	public void SetActivePhoneme(string phonemeKey)
	{
		if (phonemeKey == "."  || _activePhonemeSet == null)
		{
			_activeActor.SetDefaultMouthTexture();
		}
		else
		{
			_activeActor.SetMouthTexture(_activePhonemeSet.GetMouthShape(phonemeKey));
			_mouthTimer = 0.0f;
		}
	}
}
