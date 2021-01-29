using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Localization.Settings;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
	[SerializeField] private DialogueLineSO _dialogueLine = default;
	[SerializeField] private bool _pauseWhenClipEnds = default; //This won't work if the clip ends on the very last frame of the Timeline

	[HideInInspector] public CutsceneManager cutsceneManager;

	private bool _dialoguePlayed;

	/// <summary>
	/// Displays a line of dialogue on screen by interfacing with the <c>CutsceneManager</c>. 
	/// </summary>
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		// Get the normalized time (0 to 1) of the playhead over the current clip that is playing
		float T = (float)(playable.GetTime() / playable.GetDuration());

		// ExpressionManager sets the blink rate to be slower when the character is speaking...hence
		// this block of code
		if (_dialoguePlayed && T > 1)
		{
			cutsceneManager.ExpressionManager.SetCharacterTalkingState(_dialogueLine.Actor, false);
			return;
		}
		else
		{
			cutsceneManager.ExpressionManager.SetCharacterTalkingState(_dialogueLine.Actor, true);
		}

		// Extract the localized string
		var phonemeOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(_dialogueLine.PhonemeSentence.TableReference,
																					_dialogueLine.PhonemeSentence.TableEntryReference);

		string phonemeLine = null;
		phonemeLine = phonemeOp.Result;
		if (phonemeOp.IsDone)
		{
			phonemeLine = phonemeOp.Result;
		}
		else
		{
			phonemeOp.Completed += (op) => phonemeLine = op.Result;
		}

		string phonemeKey = null;
		if (phonemeLine != null)
		{
			// Add a "." at the start and end of the phoneme line, which will be interpreted as the default phoneme
			// mouth shape, so that the mouth starts and ends with the closed phoneme mouth shape
			string newPhonemeLine = ". " + phonemeLine + " .";

			// Now parse out the phonemes using the whitespace as separators 
			string[] newPhonemes = newPhonemeLine.Split(' ');

			// Now convert that normalized time to a normalized index in the newPhonemes array
			int index = Mathf.RoundToInt(T * (newPhonemes.Length - 1));

			// Save this phoneme key so that it can be sent to the CutsceneManager
			phonemeKey = newPhonemes[index];

		//	Debug.Log("ACTIVE PHONEME = " + phonemeKey);
		}
		else
		{
			phonemeKey = ".";
		}

		if (Application.isPlaying)  //TODO: Find a way to "play" dialogue lines even when scrubbing the Timeline not in Play Mode
		{
			// Need to ask the CutsceneManager if the cutscene is playing, since the graph is not actually stopped/paused: it's just going at speed 0.
			if (playable.GetGraph().IsPlaying()
				&& cutsceneManager.IsCutscenePlaying)
			{
				if (_dialogueLine != null)
				{
					// Send Dialogue line and phoneme key to Cutscene Manager
					if (!_dialoguePlayed)
						cutsceneManager.PlayDialogueFromClip(_dialogueLine);

					// Make sure this actor is registered with the ExpressionManager so that it can control the actor's expressions
					if (!cutsceneManager.ExpressionManager.IsActorRegistered(_dialogueLine.Actor))
						cutsceneManager.ExpressionManager.RegisterActor(_dialogueLine.Actor);

					// Set the currently parsed phoneme as the active one
					if (cutsceneManager.ExpressionManager.IsThereAnActivePhonemeSetForActor(_dialogueLine.Actor))
						cutsceneManager.ExpressionManager.SetActivePhoneme(_dialogueLine.Actor, phonemeKey);

					_dialoguePlayed = true;
				}
				else
				{
					Debug.LogWarning("This clip contains no DialogueLine");
				}
			}
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		// The check on _dialoguePlayed is needed because OnBehaviourPause is called also at the beginning of the Timeline,
		// so we need to make sure that the Timeline has actually gone through this clip (i.e. called OnBehaviourPlay) at least once before we stop it
		if (Application.isPlaying
			&& playable.GetGraph().IsPlaying()
			&& !playable.GetGraph().GetRootPlayable(0).IsDone()
			&& _dialoguePlayed)
		{
			if (_pauseWhenClipEnds)
				cutsceneManager.PauseTimeline();
		}
	}
}
