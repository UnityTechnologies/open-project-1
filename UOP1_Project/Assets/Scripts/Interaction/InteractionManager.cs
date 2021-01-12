using UnityEngine;
using System.Collections.Generic;

public enum InteractionType { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	[HideInInspector] public InteractionType currentInteraction;
	[SerializeField] private InputReader _inputReader = default;
	//To store the object we are currently interacting with
	private Stack<Interaction> _ongoingInteractions = new Stack<Interaction>();

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
		Interaction onGoingInteraction = _ongoingInteractions.Peek();
		switch (onGoingInteraction.Type)
		{
			//we show it after cooking or talking, in case player want to interact again
			case InteractionType.Cook:
			case InteractionType.Talk:
				_toggleInteractionUI.RaiseEvent(true, onGoingInteraction.Type);
				Debug.Log("Display interaction UI");
				break;
			default:
				break;
		}
	}

	void OnInteractionButtonPress()
	{
		//remove interaction after press
		Interaction onGoingInteraction = _ongoingInteractions.Count > 0 ?
			_ongoingInteractions.Peek() : Interaction.NONE;
		_toggleInteractionUI.RaiseEvent(false, onGoingInteraction.Type);
		switch (onGoingInteraction.Type)
		{
			case InteractionType.None:
				return;
			case InteractionType.PickUp:
				GameObject itemObject = _ongoingInteractions.Pop().InteractableObject;
				if (_onObjectPickUp != null)
				{
					//raise an event with an item as parameter (to add object to inventory)
					Item currentItem = itemObject.GetComponent<CollectibleItem>().GetItem();
					_onObjectPickUp.RaiseEvent(currentItem);
					//Debug.Log("PickUp event raised");
					//set current interaction for state machine
					currentInteraction = InteractionType.PickUp;
				}
				//destroy the GO
				Destroy(itemObject);
				break;
			case InteractionType.Cook:
				if (_onCookingStart != null)
				{
					_onCookingStart.RaiseEvent();
					//Debug.Log("Cooking event raised");
					//Change the action map
					_inputReader.EnableMenuInput();
					//set current interaction for state machine
					currentInteraction = InteractionType.Cook;
				}
				break;
			case InteractionType.Talk:
				if (_onCookingStart != null)
				{
					//raise an event with an actor as parameter
					//Actor currentActor = currentInteractableObject.GetComponent<Dialogue>().GetActor();
					//_startTalking.RaiseEvent(currentActor);
					//Debug.Log("talk event raised");
					//Change the action map
					_inputReader.EnableDialogueInput();
					//set current interaction for state machine
					currentInteraction = InteractionType.Talk;
				}
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		InteractionType ongoingInteractionType = InteractionType.None;

		if (other.CompareTag("Pickable"))
		{
			ongoingInteractionType = InteractionType.PickUp;
			//Debug.Log("I triggered a pickable object!");
		}
		else if (other.CompareTag("CookingPot"))
		{
			ongoingInteractionType = InteractionType.Cook;
			//Debug.Log("I triggered a cooking pot!");
		}
		else if (other.CompareTag("NPC"))
		{
			ongoingInteractionType = InteractionType.Talk;
			//Debug.Log("I triggered an NPC!");
		}
		if (ongoingInteractionType != InteractionType.None)
		{
			_ongoingInteractions.Push(new Interaction(ongoingInteractionType, other.gameObject));
			DisplayInteractionUI();
		}
	}

	private void DisplayInteractionUI()
	{
		//Raise event to display UI
		Interaction onGoingInteraction = _ongoingInteractions.Peek();
		_toggleInteractionUI.RaiseEvent(true, onGoingInteraction.Type);
	}

	private void OnTriggerExit(Collider other)
	{
		ResetInteraction(other.gameObject);
	}

	private void ResetInteraction(GameObject obj)
	{
		Stack<Interaction> updatedStack = new Stack<Interaction>();
		while(_ongoingInteractions.Count > 0)
		{
			Interaction interaction = _ongoingInteractions.Pop();
			if (interaction.InteractableObject != obj)
			{
				updatedStack.Push(interaction);
			}
		}
		_ongoingInteractions = updatedStack;

		if (_ongoingInteractions.Count > 0)
		{
			_toggleInteractionUI.RaiseEvent(true, _ongoingInteractions.Peek().Type);

		}
		else
		{
			_toggleInteractionUI.RaiseEvent(false, InteractionType.None);
		}
	}
}
