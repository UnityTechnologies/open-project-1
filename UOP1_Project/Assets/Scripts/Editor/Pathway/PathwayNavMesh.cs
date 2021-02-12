using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.AI;
using System;

public class PathwayNavMesh 
{
	private bool _toggled;
	private Pathway _pathway;
	
	public PathwayNavMesh(Pathway pathway)
	{
		_pathway = pathway;
		_toggled = false;
		_pathway.Hits = new List<Pathway.HitPoint>();
		_pathway.DisplayPolls = false;
		_pathway.Path = new Vector3[]{ };
	}

	private bool PollsNavMesh()
	{
		NavMeshHit hit;
		bool hasHit;
		bool result = true;

		_pathway.Hits.Clear();

		for (int i = 0; i < _pathway.wayPoints.Length; i++)
		{
			hasHit = NavMesh.SamplePosition(_pathway.wayPoints[i], out hit, _pathway.MeshSize * 2, NavMesh.AllAreas);
			_pathway.Hits.Add(new Pathway.HitPoint(hasHit, hit.position));
			result &= hasHit;
		}

		return result;
	}

	private bool GenerateNavMeshPath()
	{
		bool canGeneatePath = true;
		List<Vector3> path = new List<Vector3>();
		NavMeshPath navMeshPath = new NavMeshPath();

		int i = 1;
		while ( i < _pathway.wayPoints.Length && canGeneatePath)
		{
			canGeneatePath &= NavMesh.CalculatePath(_pathway.wayPoints[i - 1], _pathway.wayPoints[i], NavMesh.AllAreas, navMeshPath);
			if (canGeneatePath)
			{
				foreach (Vector3 corner in navMeshPath.corners)
				{
					path.Add(corner);
				}
			}

			i++;
		}

		if (canGeneatePath)
			_pathway.Path = path.ToArray();
		return canGeneatePath;
	}

	public void OnInspectorGUI()
	{
		if (_toggled == false)
		{
			if (_toggled = GUILayout.Button("NavMesh Path"))
			{
				if (PollsNavMesh())
				{
					if (_pathway.wayPoints.Length > 1)
					{
						if (GenerateNavMeshPath())
						{
							InternalEditorUtility.RepaintAllViews();
						}
						else
						{
							Debug.LogError("NavMesh path can't generate a path");
						}
					}
					else
						Debug.LogError("Pathway need more than one point to calculate the path");
				}
				else
				{
					_pathway.DisplayPolls = true;
				}
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				_toggled = false;
				_pathway.DisplayPolls = false;
				Array.Clear(_pathway.Path,0,_pathway.Path.Length);
				InternalEditorUtility.RepaintAllViews();
			}

			if (_pathway.DisplayPolls)
			{
				if (GUILayout.Button("Hide Polls"))
				{
					_pathway.DisplayPolls = false;
					InternalEditorUtility.RepaintAllViews();
				}
			}
			else
			{
				if (GUILayout.Button("Show polls"))
				{
					_pathway.DisplayPolls = true;
					InternalEditorUtility.RepaintAllViews();
				}
			}
		}
	}

}
