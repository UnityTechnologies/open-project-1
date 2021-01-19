using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CritterVariant", menuName = "Critter/Critter Variant")]
public class CritterSO : ScriptableObject
{
	[Tooltip("The name of the critter")]
	[SerializeField] private LocalizedString _name;

	[Tooltip("Initial critter health")]
	[SerializeField] private int _maxHealth;

	[Tooltip("How long the critter wait before roaming somewhere else (in second).")]
	[SerializeField] private float _waitTime = default;

	[Tooltip("Critter roaming speed")]
	[SerializeField] private float _roamingSpeed = default;

	[Tooltip("Critter chasing speed")]
	[SerializeField] private float _chasingSpeed = default;

	[Tooltip("How far the critter can roam around its spawning point.")]
	[SerializeField] private float _roamingDistance = default;

	[Tooltip("Player transform anchor.")]
	[SerializeField] private TransformAnchor _playerTransform = default;

	[Tooltip("Maximum number of items that can be dropped by the critter when killed.")]
	[SerializeField]
	private int _maxNbDropppedItems = 1;


	[Tooltip("The list of item that can be dropped by this critter when killed")]
	[SerializeField]
	private List<DropItem> _dropItems = new List<DropItem>();

	public LocalizedString Name => _name;
	public int MaxHealth => _maxHealth;
	public List<DropItem> DropItems => _dropItems;
	public float WaitTime => _waitTime;
	public float RoamingSpeed => _roamingSpeed;
	public float ChasingSpeed => _chasingSpeed;
	public float RoamingDistance => _roamingDistance;
	public Vector3 PlayerPosition => _playerTransform.Transform.position;

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
