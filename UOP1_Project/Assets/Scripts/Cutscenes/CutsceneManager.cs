using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
	// Singleton
	private static CutsceneManager _instance;
	public static CutsceneManager Instance { get => _instance; }

	public bool IsInteracting { get; private set; }
	public int DialogueCounter { get; private set; } 
	public PlayableDirector Director { get; private set; }

	private CutsceneData _cutsceneData; 
	[SerializeField] private InputReader _inputReader = default;

	private void Awake()
	{ 
		if(Instance == null)
		{
			_instance = this;

			// Initialize CutsceneManager.
			Director = gameObject.GetComponent<PlayableDirector>();
			_inputReader.GameInput.Menus.Advance.performed += ctx => OnAdvance();
		}
		else
		{
			Destroy(this);
		}
	}

	public int GetDialogueLength()
	{
		return _cutsceneData.DialogueData.Conversation.Count;
	}

	/// <summary>
	/// Disable gameplay input.
	/// Enable Menus input.
	/// Play Timeline if possible, if not just open dialogue box.
	/// </summary>
	/// <param name="dialogueData"></param>
	public void Play(CutsceneData cutsceneData)
	{
		IsInteracting = true;

		_cutsceneData = cutsceneData;

		if(cutsceneData.DialogueData.TimelineAsset != null)
		{
			if(Director.state != PlayState.Playing)
			{
				Director.playableAsset = cutsceneData.DialogueData.TimelineAsset;
				Director.Play();
				Director.stopped += ctx => NotAbleToInteract();
			}
		}

		Interacting(); 
	}

	/// <summary>
	/// When player able to interact with something in the world.
	/// Show interaction box
	/// Enable Menus input.
	/// </summary>
	/// <param name="posInWorld">Where interaction box should be drawed.</param>
	public void AbleToInteract(Vector3 posInWorld, CutsceneData cutsceneData)
	{
		IsInteracting = true;
		EnableInteractionInput();
		_cutsceneData = cutsceneData;
	}

	/// <summary>
	/// When player not able / stop interacting with something in the world.
	/// Hide interaction box
	/// Enable Gameplay input.
	/// </summary> 
	public void NotAbleToInteract()
	{
		Director.Stop();
		EnableGameplayInput();
		IsInteracting = false;
		DialogueCounter = 0;
	}

	/// <summary>
	/// When player is interacting with cutscene.
	/// <list type="bullet">
	///<item>Disable gameplay input</item>
	/// </list> 
	/// </summary>
	public void Interacting()
	{
		DisableGameplayInput(); 
	}

	/// <summary>
	/// When advance button clicked.
	/// <list type="bullet">
	///<item>Otherwise Increment dialogue counter.</item>
	///<item>Stop interacting </item>
	/// </list>
	/// </summary>
	private void OnAdvance()
	{
		if(DialogueCounter == 0)
		{
			Play(_cutsceneData);
		}
	}

	public void PauseTimeline()
	{
		Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
	}

	public void ResumeTimeline()
	{
		Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}

	/// <summary>
	/// Enable Menus input.
	/// Disable Gameplay.Jump since it's overlapping with Menus.Advance.
	/// </summary>
	private void EnableInteractionInput()
	{
		_inputReader.GameInput.Menus.Enable();
		_inputReader.GameInput.Gameplay.Jump.Disable();
	}

	/// <summary>
	/// Disable gameplay input and enable menus input.
	/// </summary>
	private void DisableGameplayInput()
	{
		_inputReader.GameInput.Gameplay.Disable();
		_inputReader.GameInput.Menus.Enable();
	}

	/// <summary>
	/// Enable Gameplay input and disable menus input.
	/// </summary>
	private void EnableGameplayInput()
	{
		_inputReader.GameInput.Menus.Disable();
		_inputReader.GameInput.Gameplay.Enable();
	}
}
