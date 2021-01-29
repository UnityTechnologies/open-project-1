using UnityEngine;
using UnityEngine.Localization;

public class DroppingEntity : AttackableEntity
{

	[SerializeField]
	private DroppableRewardConfigSO _droppableRewardSO;

	public DroppableRewardConfigSO DropableRewardConfig => _droppableRewardSO;
	
}
