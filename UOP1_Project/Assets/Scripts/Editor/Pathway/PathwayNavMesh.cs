using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PathwayNavMesh 
{
	private SerializedProperty _probeRadius;
	private Pathway _pathway;
	
	public PathwayNavMesh(SerializedObject serializedObject, Pathway pathway)
	{
		_pathway = pathway;
		_pathway.Hits = new List<bool>();
		_pathway.Path = new List<Vector3>();
		_probeRadius = serializedObject.FindProperty("_probeRadius");
	}

	public bool HasNavMeshAt(int index)
	{
		NavMeshHit hit;
		bool hasHit;

		hasHit = NavMesh.SamplePosition(_pathway.Waypoints[index], out hit, _probeRadius.floatValue, NavMesh.AllAreas);

		if (index > _pathway.Hits.Count - 1)
		{
			index = _pathway.Hits.Count;
			_pathway.Hits.Add(hasHit);
			
		}

		_pathway.Hits[index] = hasHit;

		if (hasHit)
		{
			_pathway.Waypoints[index] = hit.position;
			
		}

		return hasHit;
	}

	public bool HasNavMesh()
	{
		bool result = true;

		_pathway.Hits.Clear();
		
		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			result &= HasNavMeshAt(i);
		}

		return result;
	}

	private void CopyPathCorners(int startIndex, int endIndex, NavMeshPath navMeshPath, ref bool canGeneratePath) {
		
		canGeneratePath &= NavMesh.CalculatePath(_pathway.Waypoints[startIndex], _pathway.Waypoints[endIndex], NavMesh.AllAreas, navMeshPath);

		if (canGeneratePath)
		{
			for (int j = 0; j < navMeshPath.corners.Length; j++)
			{
				_pathway.Path.Add(navMeshPath.corners[j]);
			}
		}
	}

	public bool GenerateNavMeshPath()
	{
		bool canGeneatePath=true;
		int i = 1;

		NavMeshPath navMeshPath = new NavMeshPath();

		_pathway.Path.Clear();

		while ( i < _pathway.Waypoints.Count)
		{
			CopyPathCorners(i - 1, i, navMeshPath, ref canGeneatePath);
			i++;
		}

		CopyPathCorners(_pathway.Waypoints.Count - 1, 0, navMeshPath, ref canGeneatePath);

		if (!canGeneatePath)
		{
			_pathway.Path.Clear();
		}

		return canGeneatePath;
	}

}
