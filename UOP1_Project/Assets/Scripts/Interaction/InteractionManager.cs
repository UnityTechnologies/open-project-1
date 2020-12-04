using UnityEngine;

public enum Interaction { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	public Interaction _interaction;
	public InputReader inputReader;
	//To store the object we are currently interacting with
	GameObject currentInteractableObject = null;
	//Or do we want to have stg specific for every type of interaction like:
	//Item for pickup?
	//Character (or other relevant type) for talk?


	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private GameObjectEventChannelSO _OnObjectPickUp = default;
	//double check with the action name we will show on the UI (because we will not really starting cooking but showing the UI?)
	[SerializeField] private VoidEventChannelSO _OnCookingStart = default;
	[SerializeField] private GameObjectEventChannelSO _StartTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO _ToggleInteractionUI = default;
	//Check if the interaction ended
	[SerializeField] private VoidEventChannelSO _InteractionEnded = default;

	private void OnEnable()
	{
		inputReader.interactEvent += OnInteractionButtonPress;
		_InteractionEnded.OnEventRaised += OnInteractionEnd;
	}

	private void OnDisable()
	{
		inputReader.interactEvent -= OnInteractionButtonPress;
		_InteractionEnded.OnEventRaised -= OnInteractionEnd;
	}

	//When the interaction ends, we still want to display the interaction UI if we are still in the trigger zone
	void OnInteractionEnd()
	{
		inputReader.EnableGameplayInput();
		switch (_interaction)
		{
			//We don't show the interaction UI when we already picked up the object
			case Interaction.None:
			case Interaction.PickUp:
				return;
			//we show it after cooking or talking, in case player want to interact again
			case Interaction.Cook:
			case Interaction.Talk:
				_ToggleInteractionUI.RaiseEvent(true, _interaction);
				Debug.Log("Display interaction UI");
				break;
			default:
				break;
		}
	}

	void OnInteractionButtonPress()
	{
		switch (_interaction)
		{
			case Interaction.None:
				return;
			case Interaction.PickUp:
				if(currentInteractableObject != null)
				{
					//pass the item SO to the UI?
					_OnObjectPickUp.RaiseEvent(currentInteractableObject);
					Debug.Log("PickUp event raised");
				}
				
				//destroy the GO
				break;
			case Interaction.Cook:
				_OnCookingStart.RaiseEvent();
				Debug.Log("Cooking event raised");
				//Change the action map
				//inputReader.EnableUIInput();
				break;
			case Interaction.Talk:
				if (currentInteractableObject != null)
				{
					_StartTalking.RaiseEvent(currentInteractableObject);
					Debug.Log("talk event raised");
					//Change the action map
					inputReader.EnableDialogueInput();
				}
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pickable"))
		{
			_interaction = Interaction.PickUp;
			Debug.Log("I triggered a pickable object!");
			DisplayInteractionUI();
		}
		else if (other.CompareTag("CookingPot"))
		{
			_interaction = Interaction.Cook;
			Debug.Log("I triggered a cooking pot!");
			DisplayInteractionUI();
		}
		else if (other.CompareTag("NPC"))
		{
			_interaction = Interaction.Talk;
			Debug.Log("I triggered an NPC!");
			DisplayInteractionUI();
		}
		currentInteractableObject = other.gameObject;
	}

	private void DisplayInteractionUI ()
	{
		//Raise event to display UI
		_ToggleInteractionUI.RaiseEvent(true, _interaction);
	}

	private void OnTriggerExit(Collider other)
	{
		ResetInteraction();
	}

	private void ResetInteraction()
	{
		_interaction = Interaction.None;
		currentInteractableObject = null;
	}
}


//Do we need to detect the button press first of the trigger first
//If we detect trigger first, to destroy the object later we need to keep a ref 


//I think the dialogue and UI should switch the action maps? for now I do here but maybe to change later
