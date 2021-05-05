using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions, GameInput.IMenusActions
{
	[SerializeField] private EventAggregatorSO eventAggregator;

	[SerializeField] private UnityEventBusSO eventBus;

	private readonly MoveEvent _moveEvent = new MoveEvent();
	private readonly CameraMoveEvent _cameraMoveEvent = new CameraMoveEvent();

	// Assign delegate{} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

	// Gameplay
	public event UnityAction interactEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
	public event UnityAction closeInventoryEvent = delegate { };// Used to bring up the inventory
	public event UnityAction inventoryActionButtonEvent = delegate { };
	public event UnityAction pauseEvent = delegate { };

	// Shared between menus and dialogues
	public event UnityAction moveSelectionEvent = delegate { };

	// Dialogues
	public event UnityAction advanceDialogueEvent = delegate { };

	// Menus
	public event UnityAction menuMouseMoveEvent = delegate { };
	public event UnityAction menuConfirmEvent = delegate { };
	public event UnityAction menuCancelEvent = delegate { };
	public event UnityAction menuUnpauseEvent = delegate { };

	public event UnityAction<float> menuSwitchTab = delegate { };


	private GameInput gameInput;

	private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Menus.SetCallbacks(this);
			gameInput.Gameplay.SetCallbacks(this);
			gameInput.Dialogues.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				eventAggregator.Publish(AttackEvent.Event);
				break;
			case InputActionPhase.Canceled:
				eventAggregator.Publish(AttackCancelledEvent.Event);
				break;
		}
	}

	public void OnOpenInventory(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			eventAggregator.Publish(OpenInventoryEvent.Event);
	}
	public void OnCancel(InputAction.CallbackContext context)
	{

		if (context.phase == InputActionPhase.Performed)
			closeInventoryEvent.Invoke();
	}

	public void OnInventoryActionButton(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			inventoryActionButtonEvent.Invoke();

	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			interactEvent.Invoke();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				eventAggregator.Publish(JumpEvent.Event);
				break;
			case InputActionPhase.Canceled:
				eventAggregator.Publish(JumpCancelledEvent.Event);
				break;
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		_moveEvent.Movement = context.ReadValue<Vector2>();
		eventAggregator.Publish(_moveEvent);
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				eventAggregator.Publish(StartedRunningEvent.Event);
				break;
			case InputActionPhase.Canceled:
				eventAggregator.Publish(StoppedRunningEvent.Event);
				break;
		}
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			pauseEvent.Invoke();
	}

	public void OnRotateCamera(InputAction.CallbackContext context)
	{
		_cameraMoveEvent.Movement = context.ReadValue<Vector2>();
		_cameraMoveEvent.IsDeviceMouse = IsDeviceMouse(context);
		eventBus.Publish(_cameraMoveEvent);
	}

	public void OnMouseControlCamera(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				eventBus.Publish(EnableMouseControlEvent.Event);
				break;
			case InputActionPhase.Canceled:
				eventBus.Publish(DisableMouseControlEvent.Event);
				break;
		}
	}

	private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

	public void OnMoveSelection(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			moveSelectionEvent();
	}

	public void OnAdvanceDialogue(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			advanceDialogueEvent();
	}

	public void OnConfirm(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			menuConfirmEvent();
	}

	/*	public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
				menuCancelEvent();
		}*/

	public void OnMouseMove(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			menuMouseMoveEvent();
	}

	public void OnUnpause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			menuUnpauseEvent();
	}

	public void EnableDialogueInput()
	{
		gameInput.Menus.Disable();
		gameInput.Gameplay.Disable();

		gameInput.Dialogues.Enable();
	}

	public void EnableGameplayInput()
	{
		gameInput.Menus.Disable();
		gameInput.Dialogues.Disable();

		gameInput.Gameplay.Enable();
	}

	public void EnableMenuInput()
	{
		gameInput.Dialogues.Disable();
		gameInput.Gameplay.Disable();

		gameInput.Menus.Enable();
	}

	public void DisableAllInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.Menus.Disable();
		gameInput.Dialogues.Disable();
	}
	public void OnChangeTab(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			menuSwitchTab.Invoke(context.ReadValue<float>());

	}

	public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
}
