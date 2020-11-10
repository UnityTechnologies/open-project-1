using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private DialogueManager _dialogueManager = default;

	private PlayableDirector _activePlayableDirector;
	private bool _isPaused;

	public bool IsCutscenePlaying => _activePlayableDirector.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;
	private void OnEnable()
	{
		_inputReader.GameInput.Dialogues.AdvanceDialogue.performed += HandleOnAdvanceInput;
	}

	private void OnDisable()
	{
		_inputReader.GameInput.Dialogues.AdvanceDialogue.performed -= HandleOnAdvanceInput;
	}

	public void PlayCutscene(PlayableDirector activePlayableDirector)
	{
		_activePlayableDirector = activePlayableDirector;

		_isPaused = false;
		_activePlayableDirector.Play();
		_activePlayableDirector.stopped += HandlePlayableDirectorStop;

		EnableDialogueInput();
	}

	private void HandlePlayableDirectorStop(PlayableDirector director) => CutsceneEnded();

	public void CutsceneEnded()
	{
		EnableGameplayInput();
	}

	public void PlayDialogueFromClip(DialogueLineSO dialogueLine)
	{
		_dialogueManager.DisplayDialogueLine(dialogueLine);
	}

	private void HandleOnAdvanceInput(InputAction.CallbackContext ctx) => OnAdvance();

	/// <summary>
	/// This callback is executed when the player presses the button to advance dialogues. If the Timeline is currently paused due to a <c>DialogueControlClip</c>, it will resume its playback.
	/// </summary>
	private void OnAdvance()
	{
		if (_isPaused)
			ResumeTimeline();
	}

	/// <summary>
	/// Called by <c>DialogueControlClip</c> on the Timeline.
	/// </summary>
	public void PauseTimeline()
	{
		_isPaused = true;
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
	}

	public void ResumeTimeline()
	{
		_isPaused = false;
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}

	private void EnableDialogueInput()
	{
		_inputReader.GameInput.Dialogues.Enable();
		_inputReader.GameInput.Gameplay.Disable();
	}

	private void EnableGameplayInput()
	{
		_inputReader.GameInput.Gameplay.Enable();
		_inputReader.GameInput.Dialogues.Disable();
	}
}
