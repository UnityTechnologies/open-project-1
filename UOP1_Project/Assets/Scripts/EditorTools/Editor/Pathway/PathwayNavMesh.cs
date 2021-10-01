using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class PathwayNavMesh
{
	private PathwayConfigSO _pathway;

	public PathwayNavMesh(PathwayConfigSO pathway)
	{
		_pathway = pathway;
		_pathway.Hits = new List<bool>();
	}

	public bool HasNavMeshAt(int index)
	{
		NavMeshHit hit;
		bool hasHit = true;

		if (_pathway.Waypoints.Count >= _pathway.Hits.Count)
		{
			hasHit = NavMesh.SamplePosition(_pathway.Waypoints[index].waypoint, out hit, _pathway.ProbeRadius, NavMesh.AllAreas);

			if (index > _pathway.Hits.Count - 1)
			{
				index = _pathway.Hits.Count;
				_pathway.Hits.Add(hasHit);
			}
			else
			{
				_pathway.Hits[index] = hasHit;

			}

			if (hasHit)
			{
				_pathway.Waypoints[index].waypoint = hit.position;
			}
		}
		else
		{
			_pathway.Hits.RemoveAt(index);
		}

		return hasHit;
	}

	private List<Vector3> GetPathCorners(int startIndex, int endIndex)
	{
		NavMeshPath navMeshPath = new NavMeshPath();

		if (NavMesh.CalculatePath(_pathway.Waypoints[startIndex].waypoint, _pathway.Waypoints[endIndex].waypoint, NavMesh.AllAreas, navMeshPath))
		{
			return navMeshPath.corners.ToList();
		}

		else
			return null;
	}

	private bool CopyCorners(int startIndex, int endIndex)
	{
		List<Vector3> result;

		if ((result = GetPathCorners(startIndex, endIndex)) != null)
		{
			_pathway.Waypoints[startIndex].corners = result;
		}

		return result != null;
	}

	public bool UpdateCornersAt(int index)
	{
		bool canUpdate = true;

		if (_pathway.Waypoints.Count > 1 && index < _pathway.Waypoints.Count)
		{
			if (index == 0)
			{
				canUpdate = CopyCorners(index, index + 1);
				canUpdate &= CopyCorners(_pathway.Waypoints.Count - 1, index);
			}
			else if (index == _pathway.Waypoints.Count - 1)
			{

				canUpdate = CopyCorners(index - 1, index);
				canUpdate &= CopyCorners(index, 0);
			}
			else
			{
				canUpdate = CopyCorners(index - 1, index);
				canUpdate &= CopyCorners(index, index + 1);
			}
		}

		return canUpdate;
	}

	public void UpdatePath()
	{
		if (_pathway.Waypoints.Count > 1)
		{
			_pathway.Path = _pathway.Waypoints.Aggregate(new List<Vector3>(), (acc, wpd) =>
			{
				wpd.corners.ForEach(c => acc.Add(c));
				return acc;
			});
		}
		else
		{
			_pathway.Path.Clear();
		}
	}

}
