using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "RoamingAroundSpawningPosition", menuName = "Characters/Roaming Around Spawning Position NPC Movement")]
public class RoamingAroundSpawningPositionConfigSO : ScriptableObject
{
	[Tooltip("How long the NPC wait before roaming somewhere else (in second).")]
	[SerializeField] private float _stopDuration = default;

	[Tooltip("NPC roaming speed.")]
	[SerializeField] private float _roamingSpeed = default;

	[Tooltip("How far the NPC can roam around its spawning point.")]
	[SerializeField] private float _roamingDistance = default;

	public float StopDuration => _stopDuration;
	public float RoamingSpeed => _roamingSpeed;
	public float RoamingDistance => _roamingDistance;

}
