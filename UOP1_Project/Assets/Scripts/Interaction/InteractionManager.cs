using UnityEngine;

public enum InteractionType { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	private InteractionType _potentialInteraction;
	[HideInInspector] public InteractionType currentInteraction;
	[SerializeField] private InputReader _inputReader = default;
	//To store the object we are currently interacting with
	private GameObject _currentInteractableObject = null;

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private ItemEventChannelSo _onObjectPickUp = default;
	[SerializeField] private VoidEventChannelSO _onCookingStart = default;
	[SerializeField] private DialogueEventChannelSo _startTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO _toggleInteractionUI = default;
	[Header("Listening to")]
	//Check if the interaction ended 
	[SerializeField] private VoidEventChannelSO _onInteractionEnded = default;

	private void OnEnable()
	{
		_inputReader.interactEvent += OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised += OnInteractionEnd;
	}

	private void OnDisable()
	{
		_inputReader.interactEvent -= OnInteractionButtonPress;
		_onInteractionEnded.OnEventRaised -= OnInteractionEnd;
	}

	//When the interaction ends, we still want to display the interaction UI if we are still in the trigger zone
	void OnInteractionEnd()
	{
		_inputReader.EnableGameplayInput();
		switch (_potentialInteraction)
		{
			//we show it after cooking or talking, in case player want to interact again
			case InteractionType.Cook:
			case InteractionType.Talk:
				_toggleInteractionUI.RaiseEvent(true, _potentialInteraction);
				Debug.Log("Display interaction UI");
				break;
			default:
				break;
		}
	}

	void OnInteractionButtonPress()
	{
		//remove interaction after press 
		_toggleInteractionUI.RaiseEvent(false, _potentialInteraction);

		switch (_potentialInteraction)
		{
			case InteractionType.None:
				return;
			case InteractionType.PickUp:
				if (_currentInteractableObject != null)
				{
					if (_onObjectPickUp != null)
					{
						//raise an event with an item as parameter (to add object to inventory)
						Item currentItem = _currentInteractableObject.GetComponent<CollectibleItem>().GetItem();
						_onObjectPickUp.RaiseEvent(currentItem);
						Debug.Log("PickUp event raised");
						//set current interaction for state machine
						currentInteraction = InteractionType.PickUp;
					}
				}
				//destroy the GO
				Destroy(_currentInteractableObject);
				break;
			case InteractionType.Cook:
				if (_onCookingStart != null)
				{
					_onCookingStart.RaiseEvent();
					Debug.Log("Cooking event raised");
					//Change the action map
					_inputReader.EnableMenuInput();
					//set current interaction for state machine
					currentInteraction = InteractionType.Cook;
				}
				break;
			case InteractionType.Talk:
				if (_currentInteractableObject != null)
				{
					if (_onCookingStart != null)
					{
						//raise an event with an actor as parameter
						//Actor currentActor = currentInteractableObject.GetComponent<Dialogue>().GetActor();
						//_startTalking.RaiseEvent(currentActor);
						Debug.Log("talk event raised");
						//Change the action map
						_inputReader.EnableDialogueInput();
						//set current interaction for state machine
						currentInteraction = InteractionType.Talk;
					}
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
			_potentialInteraction = InteractionType.PickUp;
			//Debug.Log("I triggered a pickable object!");
			DisplayInteractionUI();
		}
		else if (other.CompareTag("CookingPot"))
		{
			_potentialInteraction = InteractionType.Cook;
			//Debug.Log("I triggered a cooking pot!");
			DisplayInteractionUI();
		}
		else if (other.CompareTag("NPC"))
		{
			_potentialInteraction = InteractionType.Talk;
			//Debug.Log("I triggered an NPC!");
			DisplayInteractionUI();
		}
		_currentInteractableObject = other.gameObject;
	}

	private void DisplayInteractionUI()
	{
		//Raise event to display UI
		_toggleInteractionUI.RaiseEvent(true, _potentialInteraction);
	}

	private void OnTriggerExit(Collider other)
	{
		ResetInteraction();
	}

	private void ResetInteraction()
	{
		_potentialInteraction = InteractionType.None;
		_currentInteractableObject = null;

		if (_toggleInteractionUI != null)
			_toggleInteractionUI.RaiseEvent(false, _potentialInteraction);
	}
}
