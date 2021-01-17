using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CritterVariant", menuName = "Critter/Critter Variant")]
public class CritterSO : ScriptableObject
{
	[Tooltip("The name of the critter")]
	[SerializeField]
	private LocalizedString _name;

	[Tooltip("Initial critter health")]
	[SerializeField]
	private int _maxHealth;


	[Tooltip("Maximum number of items that can be dropped by the critter when killed.")]
	[SerializeField]
	private int _maxNbDropppedItems = 1;


	[Tooltip("The list of item that can be dropped by this critter when killed")]
	[SerializeField]
	private List<DropItem> _dropItems = new List<DropItem>();

	public LocalizedString Name => _name;
	public int MaxHealth => _maxHealth;
	public List<DropItem> DropItems => _dropItems;

	public int GetNbDroppedItems()
	{
		return Mathf.CeilToInt(Random.Range(0.0f, _maxNbDropppedItems));
	}

	public Item GetDroppedItem()
	{
		float dropDice = Random.value;
		float _currentRate = 0.0f;
		foreach (DropItem dropItem in _dropItems)
		{
			_currentRate += dropItem.DropRate;
			if (_currentRate >= dropDice)
			{
				return dropItem.Item;
			}
		}
		return null;
	}
}
