using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathwayConfig", menuName = "EntityConfig/Pathway Config")]
public class PathwayConfigSO : NPCMovementConfigSO
{
	[Tooltip("Pathway waypoints")]
	[SerializeField] private List<Vector3> _waypoints;

	public List<Vector3> Waypoints => _waypoints;

}
