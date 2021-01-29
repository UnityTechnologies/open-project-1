using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroppableRewardConfig", menuName = "EntityConfig/Reward Dropping Rate Config")]
public class DroppableRewardConfigSO : ScriptableObject
{
	[Tooltip("Item scattering distance from the source of dropping.")]
	[SerializeField]
	private float _scatteringDistance = default;

	[Tooltip("The list of drop goup that can be dropped by this critter when killed")]
	[SerializeField]
	private List<DropGroup> _dropGroups = new List<DropGroup>();

	public float ScatteringDistance => _scatteringDistance;
	public List<DropGroup> DropGroups => _dropGroups;

}
