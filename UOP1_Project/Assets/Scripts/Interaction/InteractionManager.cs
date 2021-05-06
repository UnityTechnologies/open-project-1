using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	[HideInInspector] public InteractionType currentInteractionType; //This is checked by conditions in the StateMachine
	[SerializeField] private InputReader _inputReader = default;
	//To store the object we are currently interacting with
	private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>();

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private ItemEventChannelSO _onObjectPickUp = default;
	[SerializeField] private VoidEventChannelSO _onCookingStart = default;
	[SerializeField] private DialogueActorChannelSO _startTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO _toggleInteractionUI = default;

	[Header("Listening to")]
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

	// Called mid-way through the AnimationClip of collecting
	private void Collect()
	{
		GameObject itemObject = _potentialInteractions.First.Value.interactableObject;
		_potentialInteractions.RemoveFirst();

		if (_onObjectPickUp != null)
		{
			Item currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
			_onObjectPickUp.RaiseEvent(currentItem);
		}

		Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX

		RequestUpdateUI(false);
	}

	private void OnInteractionButtonPress()
	{
		if (_potentialInteractions.Count == 0)
			return;

		currentInteractionType = _potentialInteractions.First.Value.type;

		switch (_potentialInteractions.First.Value.type)
		{
			case InteractionType.Cook:
				if (_onCookingStart != null)
				{
					_onCookingStart.RaiseEvent();
					_inputReader.EnableMenuInput();
				}
				break;

			case InteractionType.Talk:
				if (_startTalking != null)
				{
					_potentialInteractions.First.Value.interactableObject.GetComponent<StepController>().InteractWithCharacter();
					_inputReader.EnableDialogueInput();
				}
				break;

				//No need to do anything for Pickup type, the StateMachine will transition to the state
				//and then the AnimationClip will call Collect()
		}
	}

	//Called by the Event on the trigger collider on the child GO called "InteractionDetector"
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		if (entered)
			AddPotentialInteraction(obj);
		else
			RemovePotentialInteraction(obj);
	}

	private void AddPotentialInteraction(GameObject obj)
	{
		Interaction newPotentialInteraction = new Interaction(InteractionType.None, obj);

		if (obj.CompareTag("Pickable"))
		{
			newPotentialInteraction.type = InteractionType.PickUp;
		}
		else if (obj.CompareTag("CookingPot"))
		{
			newPotentialInteraction.type = InteractionType.Cook;
		}
		else if (obj.CompareTag("NPC"))
		{
			newPotentialInteraction.type = InteractionType.Talk;
		}

		if (newPotentialInteraction.type != InteractionType.None)
		{
			_potentialInteractions.AddFirst(newPotentialInteraction);
			RequestUpdateUI(true);
		}
	}

	private void RemovePotentialInteraction(GameObject obj)
	{
		LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
		while (currentNode != null)
		{
			if (currentNode.Value.interactableObject == obj)
			{
				_potentialInteractions.Remove(currentNode);
				break;
			}
			currentNode = currentNode.Next;
		}

		RequestUpdateUI(_potentialInteractions.Count > 0);
	}

	private void RequestUpdateUI(bool visible)
	{
		if (visible)
			_toggleInteractionUI.RaiseEvent(true, _potentialInteractions.First.Value.type);
		else
			_toggleInteractionUI.RaiseEvent(false, InteractionType.None);
	}

	private void OnInteractionEnd()
	{
		switch (currentInteractionType)
		{
			case InteractionType.Cook:
			case InteractionType.Talk:
				//We show the UI after cooking or talking, in case player wants to interact again
				RequestUpdateUI(true);
				break;
		}

		_inputReader.EnableGameplayInput();
	}
}
