using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "DroppableReward", menuName = "Droppable/Reward Dropping Rate")]
public class DroppableRewardSO : ScriptableObject
{
	[Tooltip("Item scattering distance from the source of dropping.")]
	[SerializeField]
	private float _scatteringDistance = default;

	[Tooltip("The list of drop goup that can be dropped by this critter when killed")]
	[SerializeField]
	private List<DropGroup> _dropItems = new List<DropGroup>();

	public void DropReward(Vector3 postion)
	{
		// Drop items
		for (int i = 0; i < _dropItems.Count; i++)
		{
			float randValue = Random.value;
			DropGroup dropGroup = _dropItems[i];
			if (dropGroup.DropRate >= randValue)
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
						item =  dropItem.Item;
						itemPrefab = dropItem.CollectibleItemPrefab;
					}
				}

				float randAngle = Random.value * Mathf.PI * 2;

				GameObject collectibleItem = GameObject.Instantiate(itemPrefab,
					postion + itemPrefab.transform.localPosition +
					_scatteringDistance * (Mathf.Cos(randAngle) * Vector3.forward + Mathf.Sin(randAngle) * Vector3.right),
					Quaternion.identity);
				collectibleItem.GetComponent<CollectibleItem>().CurrentItem = item;
			}
			else
			{
				break;
			}

			
		}
	}
}
