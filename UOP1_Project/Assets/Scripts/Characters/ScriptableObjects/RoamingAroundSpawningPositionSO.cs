using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "RoamingAroundSpawningPosition", menuName = "Characters/Roaming Around Spawning Position NPC Movement")]
public class RoamingAroundSpawningPositionSO : NpcMovementSO
{
	[Tooltip("How long the NPC wait before roaming somewhere else (in second).")]
	[SerializeField] internal float _waitTime = default;

	[Tooltip("NPC roaming speed.")]
	[SerializeField] internal float _roamingSpeed = default;

	[Tooltip("How far the NPC can roam around its spawning point.")]
	[SerializeField] internal float _roamingDistance = default;

	public override NpcMovementData CreateNpcMovementData(GameObject obj)
	{
		RoamingAroundSpawningPositionInstanceData data = new RoamingAroundSpawningPositionInstanceData();

		data.agent = obj.GetComponent<NavMeshAgent>();
		data.startPosition = obj.transform.position;
		data.currentWaitTime = _waitTime;
		data.roamingPosTarget = GetRoamingPositionAroundPosition(data.startPosition);
		
		return data;
	}

	public override void NpcMovementUpdate(NpcMovementData data)
	{
		RoamingAroundSpawningPositionInstanceData roamingData = (RoamingAroundSpawningPositionInstanceData)data;
		roamingData.agent.speed = _roamingSpeed;
		roamingData.agent.SetDestination(roamingData.roamingPosTarget);
		if (!roamingData.agent.hasPath)
		{
			roamingData.currentWaitTime -= Time.deltaTime;
			// Have a short rest at destination before roaming somewhere else.
			if (roamingData.currentWaitTime < 0)
			{
				roamingData.roamingPosTarget = GetRoamingPositionAroundPosition(roamingData.startPosition);
				roamingData.currentWaitTime = _waitTime;
			}
		}
	}

	// Compute a random target position around the starting position.
	internal Vector3 GetRoamingPositionAroundPosition(Vector3 position)
	{
		return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_roamingDistance / 2, _roamingDistance);
	}
}

public class RoamingAroundSpawningPositionInstanceData : NpcMovementData
{
	internal NavMeshAgent agent;
	internal float currentWaitTime = default;
	internal Vector3 startPosition = default;
	internal Vector3 roamingPosTarget = default;
}
