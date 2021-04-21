using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoamingAroundCenter", menuName = "EntityConfig/Roaming Around Center")]
public class RoamingAroundCenterConfigSO : NPCMovementConfigSO
{
	[Tooltip("Is roaming from spwaning center")]
	[SerializeField] private bool _fromSpawningPoint = true;

	[Tooltip("Custom roaming center")]
	[SerializeField] private Vector3 _customCenter;

	[Tooltip("Roaming distance from center")]
	[SerializeField] private float _radius;

	public bool FromSpawningPoint => _fromSpawningPoint;
	public Vector3 CustomCenter => _customCenter;
	public float Radius => _radius;
}
