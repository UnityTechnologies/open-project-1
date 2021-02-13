﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.AI;


public class PathwayNavMesh 
{
	private Pathway _pathway;
	private bool _toggled;
	public PathwayNavMesh(Pathway pathway)
	{
		_pathway = pathway;
		_toggled = false;
		_pathway.DisplayPolls = false;
		_pathway.TogglePathDisplay = false;
		_pathway.Path = new List<Vector3>();
		_pathway.Hits = new List<Pathway.HitPoint>();
	}

	private bool PollsNavMesh()
	{
		NavMeshHit hit;
		bool hasHit;
		bool result = true;

		_pathway.Hits.Clear();

		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			hasHit = NavMesh.SamplePosition(_pathway.Waypoints[i], out hit, _pathway.MeshSize * 2, NavMesh.AllAreas);
			_pathway.Hits.Add(new Pathway.HitPoint(hasHit, hit.position));
			result &= hasHit;
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
		else
		{
			Debug.LogError("the path between " + startIndex + " and " + endIndex + " can't be generated by NavMeshPAth");
		}
	}

	private bool GenerateNavMeshPath()
	{
		bool canGeneatePath=true;
		int i = 1;
		NavMeshPath navMeshPath = new NavMeshPath();
		
		while ( i < _pathway.Waypoints.Count)
		{
			CopyPathCorners(i - 1, i, navMeshPath, ref canGeneatePath);
			i++;
		}

		CopyPathCorners(_pathway.Waypoints.Count-1, 0, navMeshPath, ref canGeneatePath);

		if (!canGeneatePath)
		{
			_pathway.Path.Clear();
		}

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
					if (_pathway.Waypoints.Count > 1)
					{
						if (GenerateNavMeshPath())
						{
							_pathway.TogglePathDisplay = true;
							InternalEditorUtility.RepaintAllViews();
						}
						else
						{
							_toggled = false;
							_pathway.TogglePathDisplay = false;
						}

					}
					else
					{
						Debug.LogError("Pathway need more than one point to calculate the path");
						_toggled = false;
						_pathway.TogglePathDisplay = false;
					}
				}
				else
				{
					_pathway.TogglePathDisplay = false;
					_pathway.DisplayPolls = true;
					InternalEditorUtility.RepaintAllViews();
				}
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				_toggled = false;
				_pathway.TogglePathDisplay = false;
				_pathway.DisplayPolls = false;
				_pathway.Path.Clear();
				InternalEditorUtility.RepaintAllViews();
			}

			if (_pathway.Waypoints.Count > 1)
			{
				if (_pathway.DisplayPolls)
				{
					if (GUILayout.Button("Hide Polls"))
					{
						_pathway.DisplayPolls = false;
						InternalEditorUtility.RepaintAllViews();
					}

					if (GUILayout.Button("Refresh Polls"))
					{
						PollsNavMesh();
						InternalEditorUtility.RepaintAllViews();
					}
				}
				else
				{
					if (GUILayout.Button("Show Polls"))
					{
						_pathway.DisplayPolls = true;
						InternalEditorUtility.RepaintAllViews();
					}
				}
			}
			else
			{
				_toggled = false;
				_pathway.TogglePathDisplay = false;
				_pathway.DisplayPolls = false;
			}
		}
	}

}
