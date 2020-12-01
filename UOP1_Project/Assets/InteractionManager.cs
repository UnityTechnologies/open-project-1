using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Enum of the possible Interaction types, we can add later if needed
//None is the default value as its value is '0'
enum Interaction { None = 0,PickUp, Cook, Talk}; 

public class InteractionManager : MonoBehaviour
{

	public InputReader inputReader;
	private Interaction _interactionType;
	//To store the object we are currently interacting with
	GameObject currentInteractableObject;

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private GameObjectEventChannelSO _OnObjectPickUp = default;
	//double check with the action name we will show on the UI (because we will not really starting cooking but showing the UI?)
	[SerializeField] private VoidEventChannelSO _OnCookingStart = default; 
	[SerializeField] private GameObjectEventChannelSO _StartTalking = default;


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
		switch (_interactionType)
		{
			case Interaction.None:
				return;
			case Interaction.PickUp:
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
		ResetInteraction();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pickable "))
		{
			_interactionType = Interaction.PickUp;
			currentInteractableObject = other.gameObject;
		}
		else if (other.CompareTag("CookingPot"))
		{
			_interactionType = Interaction.Cook;
		}
		else if (other.CompareTag("NPC"))
		{
			_interactionType = Interaction.Talk;
			currentInteractableObject = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		ResetInteraction();
	}

	private void ResetInteraction()
	{
		_interactionType = Interaction.None;
		currentInteractableObject = null;
	}
}
