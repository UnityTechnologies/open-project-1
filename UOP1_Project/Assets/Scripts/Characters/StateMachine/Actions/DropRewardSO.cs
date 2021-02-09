using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DropReward", menuName = "State Machines/Actions/Drop Reward")]
public class DropRewardSO : StateActionSO
{
	protected override StateAction CreateAction() => new DropReward();
}

public class DropReward : StateAction
{
	private DroppableRewardConfigSO _dropRewardConfig;
	private Transform _currentTransform;

	public override void Awake(StateMachine stateMachine)
	{
		_dropRewardConfig = stateMachine.GetComponent<Damageable>().DropableRewardConfig;
		_currentTransform = stateMachine.transform;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		DropAllRewards(_currentTransform.position);
	}

	private void DropAllRewards(Vector3 postion)
	{
		// Drop items
		foreach (DropGroup dropGroup in _dropRewardConfig.DropGroups)
		{
			float randValue = Random.value;
			if (dropGroup.DropRate >= randValue)
			{
				DropOneReward(dropGroup, postion);
			}
			else
			{
				break;
			}
		}
	}

	private void DropOneReward(DropGroup dropGroup, Vector3 postion)
	{
		float dropDice = Random.value;
		float _currentRate = 0.0f;

		Item item = null;
		GameObject itemPrefab = null;

		foreach (DropItem dropItem in dropGroup.Drops)
		{
			_currentRate += dropItem.ItemDropRate;
			if (_currentRate >= dropDice)
			{
				item = dropItem.Item;
				itemPrefab = dropItem.Item.Prefab;
				break;
			}
		}

		float randAngle = Random.value * Mathf.PI * 2;
		GameObject collectibleItem = GameObject.Instantiate(itemPrefab,
			postion + itemPrefab.transform.localPosition +
			_dropRewardConfig.ScatteringDistance * (Mathf.Cos(randAngle) * Vector3.forward + Mathf.Sin(randAngle) * Vector3.right),
			Quaternion.identity);
		collectibleItem.GetComponent<CollectibleItem>().SetItem(item);
	}
}
