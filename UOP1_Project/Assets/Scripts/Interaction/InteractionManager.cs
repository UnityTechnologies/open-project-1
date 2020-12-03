using UnityEngine;

public enum Interaction { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	public Interaction _interaction;
	public InputReader inputReader;
	//To store the object we are currently interacting with
	GameObject currentInteractableObject;
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


	private void OnEnable()
	{
		inputReader.interactEvent += OnInteractionButtonPress;
	}

	private void OnDisable()
	{
		inputReader.interactEvent -= OnInteractionButtonPress;
	}

	void OnInteractionButtonPress()
	{
		switch (_interaction)
		{
			case Interaction.None:
				return;
			case Interaction.PickUp:
				//Maybe better add check if gb not null here?
				//pass the item SO to the UI
				//destroy the GO
				//Change the action map 
				_OnObjectPickUp.RaiseEvent(currentInteractableObject);
				Debug.Log("PickUp event raised");
				break;
			case Interaction.Cook:
				_OnCookingStart.RaiseEvent();
				Debug.Log("Cooking event raised");
				break;
			case Interaction.Talk:
				_StartTalking.RaiseEvent(currentInteractableObject);
				Debug.Log("talk event raised");
				break;
			default:
				break;
		}
		//ResetInteraction();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pickable"))
		{
			_interaction = Interaction.PickUp;
			currentInteractableObject = other.gameObject;
			Debug.Log("I triggered a pickable object!");
			//Raise event to display UI 
			_ToggleInteractionUI.RaiseEvent(true, _interaction);
		}
		else if (other.CompareTag("CookingPot"))
		{
			_interaction = Interaction.Cook;
			//Raise event to display UI or have a ref de display it from here
			Debug.Log("I triggered a cooking pot!");
		}
		else if (other.CompareTag("NPC"))
		{
			_interaction = Interaction.Talk;
			currentInteractableObject = other.gameObject;
			//Raise event to display UI or have a ref de display it from here
			Debug.Log("I triggered an NPC!");
		}
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
