﻿using UnityEditor;
using UnityEngine.AI;


public class PathwayNavMesh 
{
	private SerializedProperty _path;
	private SerializedProperty _hits;
	private SerializedProperty _waypoints;
	private SerializedProperty _probeRadius;

	public PathwayNavMesh(SerializedObject serializedObject)
	{
		_hits = serializedObject.FindProperty("Hits");
		_path = serializedObject.FindProperty("Path");
		_waypoints = serializedObject.FindProperty("Waypoints");
		_probeRadius = serializedObject.FindProperty("_probeRadius");
	}

	public bool hasNavMesh()
	{
		NavMeshHit hit;
		bool hasHit;
		bool result = true;

		_hits.ClearArray();

		for (int i = 0; i < _waypoints.arraySize; i++)
		{
			hasHit = NavMesh.SamplePosition(_waypoints.GetArrayElementAtIndex(i).vector3Value, out hit, _probeRadius.floatValue, NavMesh.AllAreas);
			_hits.InsertArrayElementAtIndex(i);
			_hits.GetArrayElementAtIndex(i).boolValue = hasHit;
			if (hasHit)
			{
				_waypoints.GetArrayElementAtIndex(i).vector3Value= hit.position;
			}
			result &= hasHit;
		}

		return result;
	}

	private void CopyPathCorners(int startIndex, int endIndex, NavMeshPath navMeshPath, ref bool canGeneratePath) {
		
		canGeneratePath &= NavMesh.CalculatePath(_waypoints.GetArrayElementAtIndex(startIndex).vector3Value, _waypoints.GetArrayElementAtIndex(endIndex).vector3Value, NavMesh.AllAreas, navMeshPath);

		if (canGeneratePath)
		{
			int index;
			for (int j = 0; j < navMeshPath.corners.Length; j++)
			{
				index = _path.arraySize;
				_path.InsertArrayElementAtIndex(_path.arraySize);
				_path.GetArrayElementAtIndex(index).vector3Value = navMeshPath.corners[j];
			}
		}
		
	}

	public bool GenerateNavMeshPath()
	{
		bool canGeneatePath=true;
		int i = 1;

		NavMeshPath navMeshPath = new NavMeshPath();
		_path.ClearArray();

		while ( i < _waypoints.arraySize)
		{
			CopyPathCorners(i - 1, i, navMeshPath, ref canGeneatePath);
			i++;
		}

		CopyPathCorners(_waypoints.arraySize-1, 0, navMeshPath, ref canGeneatePath);

		if (!canGeneatePath)
		{
			_path.ClearArray();
		}

		return canGeneatePath;
	}

}
