using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathwayMovementAction : NPCMovementAction
{
	private NavMeshAgent _agent;
	private bool _isActiveAgent;
	private List<WaypointData> _wayppoints;
	private int _wayPointIndex;
	private float _roamingSpeed;

	public PathwayMovementAction(
		PathwayConfigSO config, NavMeshAgent agent)
	{
		_agent = agent;
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
		_wayPointIndex = - 1; //Initialized to -1 so we don't skip the first element from the waypoint list
		_roamingSpeed = config.Speed;
		_wayppoints = config.Waypoints;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_agent.speed = _roamingSpeed;
			_agent.isStopped = false;
			_agent.SetDestination(GetNextDestination());
		}
	}

	public override void OnStateExit()
	{

	}

	private Vector3 GetNextDestination()
	{
		Vector3 nextDestination = _agent.transform.position;
		if (_wayppoints.Count > 0)
		{
			//We check the modulo so when we reach the end of the array we go back to the first element
			_wayPointIndex = (_wayPointIndex + 1) % _wayppoints.Count;
			nextDestination = _wayppoints[_wayPointIndex].waypoint;
		}
		//Debug.Log("the next destination index = " +_wayPointIndex + "value = " + nextDestination);
		return nextDestination;
	}
}
