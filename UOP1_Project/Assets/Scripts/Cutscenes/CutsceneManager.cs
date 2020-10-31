using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour
{
	// Singleton
	public static CutsceneManager Instance;

	public bool IsInteracting { get; private set; }
	public int _dialogueCounter { get; private set; }

	[SerializeField] private InputReader _inputReader;

	#region UI Related
	[SerializeField] private DialogueBox _normalDialogueBox;
	[SerializeField] private GameObject _interactionBox;
	[SerializeField] private Image _image;   // To fade in or fade out screen.
	#endregion

	#region Fields
	public PlayableDirector director;
	private CutsceneData _cutsceneData;
	
	#endregion

	// Singleton DP
	private void Awake()
	{ 
		if(Instance == null)
		{
			Instance = this;

			// Initialize CutsceneManager.
			director = gameObject.GetComponent<PlayableDirector>();
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
			if(director.state != PlayState.Playing)
			{
				director.playableAsset = cutsceneData.DialogueData.TimelineAsset;
				director.Play();
				director.stopped += ctx => NotAbleToInteract();
			}
		}
		else
		{
			OpenDialogueBox(true);
		}

		Interacting(); 
	}

	public void PauseTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(0); 
	}

	public void ResumeTimeline()
	{
		director.playableGraph.GetRootPlayable(0).SetSpeed(1);
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
		ShowInteractionBox(posInWorld);
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
		director.Stop();
		EnableGameplayInput();
		CloseDialogueBox();
		IsInteracting = false;
		_dialogueCounter = 0;
		DisableInteractionBox();
	}

	/// <summary>
	/// When player is interacting with cutscene.
	/// <list type="bullet">
	///<item>Disable interaction box.</item>
	///<item>Disable gameplay input</item>
	/// </list> 
	/// </summary>
	public void Interacting()
	{
		DisableInteractionBox();

		DisableGameplayInput(); 
	}

	/// <summary>
	/// When advance button clicked.
	/// <list type="bullet">
	///<item>Show dialogue box if counter is 0.</item>
	///<item>Otherwise Increment dialogue counter.</item>
	///<item>Stop interacting </item>
	/// </list>
	/// </summary>
	private void OnAdvance()
	{
		if(_dialogueCounter == 0)
		{
			Play(_cutsceneData);
		}

		if (_normalDialogueBox.Box.activeInHierarchy)
		{
			if (_dialogueCounter < _cutsceneData.DialogueData.Conversation.Count - 1)
			{
				AdvanceDialogueBox();
			}
			else
			{
				NotAbleToInteract();
			}
		} 
	}

	#region Input related methods.
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
	#endregion

	#region InteractionBox methods.
	/// <summary>
	/// Show small box indicating that player can interact
	/// </summary>
	private void ShowInteractionBox(Vector3 posInWorld)
	{
		_interactionBox.transform.position = Camera.main.WorldToScreenPoint(posInWorld);
		_interactionBox.SetActive(true);
	}

	private void DisableInteractionBox()
	{
		_interactionBox.SetActive(false);
	}
	#endregion

	#region DialogueBox methods.
	public void OpenDialogueBox(bool condition)
	{
		if (condition)
			UpdateDialogueBox();
		else
			CloseDialogueBox();
	}

	private void AdvanceDialogueBox()
	{
		_dialogueCounter++;
		UpdateDialogueBox();
	}

	private void CloseDialogueBox()
	{
		_normalDialogueBox.Box.SetActive(false);
	}

	private void UpdateDialogueBox()
	{
		DetermineWhichNameAndFaceShouldBeUsed();
		_normalDialogueBox.Message.text = _cutsceneData.DialogueData.Conversation[_dialogueCounter].Sentence;
		_normalDialogueBox.Box.SetActive(true);
	}

	/// <summary>
	/// Determine whether to use the actor data or the override data.
	/// </summary>
	private void DetermineWhichNameAndFaceShouldBeUsed()
	{
		// Reset sprite
		_normalDialogueBox.Image.sprite = null;

		// Use Actor Data
		if (_cutsceneData.DialogueData.Conversation[_dialogueCounter].Actor != null)
		{ 
			_normalDialogueBox.Name.text = _cutsceneData.DialogueData.Conversation[_dialogueCounter].Actor.ActorName;
			_normalDialogueBox.Image.sprite = _cutsceneData.DialogueData.Conversation[_dialogueCounter].Actor.Face; 
		} 

		// Override
		if (_cutsceneData.DialogueData.Conversation[_dialogueCounter].NameOverride != String.Empty)
		{
			_normalDialogueBox.Name.text = _cutsceneData.DialogueData.Conversation[_dialogueCounter].NameOverride;
		}

		if (_cutsceneData.DialogueData.Conversation[_dialogueCounter].FigureOverride != null)
		{
			_normalDialogueBox.Image.sprite = _cutsceneData.DialogueData.Conversation[_dialogueCounter].FigureOverride;
		}

		// Disable figure if sprite is null
		if(_normalDialogueBox.Image.sprite == null)
		{
			_normalDialogueBox.Image.gameObject.SetActive(false);
		}
		else
		{
			_normalDialogueBox.Image.gameObject.SetActive(true);
		}
	}
	#endregion
}

[Serializable]
public class DialogueBox
{
	public GameObject Box;
	public TMP_Text Name;
	public TMP_Text Message;
	public Image Image;
}
