using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Picking Up")]
public class IsPickingUpSO : StateConditionSO<IsPickingUpCondition> { }

public class IsPickingUpCondition : Condition
{
	//Component references
	private InteractionManager _interactScript;

	public override void Awake(StateMachine stateMachine)
	{
		_interactScript = stateMachine.GetComponent<InteractionManager>();
	}

	protected override bool Statement()
	{
		if (_interactScript.currentInteractionType == InteractionType.PickUp)
		{
			// Consume it
			_interactScript.currentInteractionType = InteractionType.None;

			return true;
		}
		else
		{
			return false;
		}
	}
}
