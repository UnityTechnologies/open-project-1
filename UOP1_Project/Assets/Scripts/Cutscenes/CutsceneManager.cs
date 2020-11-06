using UnityEngine;
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
		_inputReader.gameInput.Dialogues.AdvanceDialogue.performed += ctx => OnAdvance();
	}

	private void OnDisable()
	{
		_inputReader.gameInput.Dialogues.AdvanceDialogue.performed -= ctx => OnAdvance();
	}

	public void PlayCutscene(PlayableDirector activePlayableDirector)
	{
		_activePlayableDirector = activePlayableDirector;

		_isPaused = false;
		_activePlayableDirector.Play();
		_activePlayableDirector.stopped += ctx => CutsceneEnded();

		EnableDialogueInput();
	}

	public void CutsceneEnded()
	{
		EnableGameplayInput();
	}

	public void PlayDialogueFromClip(DialogueLineSO dialogueLine)
	{
		_dialogueManager.DisplayDialogueLine(dialogueLine);
	}

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
		_inputReader.gameInput.Dialogues.Enable();
		_inputReader.gameInput.Gameplay.Disable();
	}

	private void EnableGameplayInput()
	{
		_inputReader.gameInput.Gameplay.Enable();
		_inputReader.gameInput.Dialogues.Disable();
	}
}
