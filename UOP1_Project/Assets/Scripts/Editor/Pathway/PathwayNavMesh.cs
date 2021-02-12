using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.AI;



public class PathwayNavMesh 
{
	private bool _toggled;
	public Pathway _pathway;

	public PathwayNavMesh(Pathway pathway)
	{
		_pathway = pathway;
		_toggled = false;
		_pathway.Path = new NavMeshPath();
		_pathway.Hits = new List<Pathway.HitPoint>();
		_pathway.DisplayPolls = false;
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

	private void GenerateNavMeshPath()
	{

		for (int i = 1; i < _pathway.wayPoints.Length; i++)
		{
			NavMesh.CalculatePath(_pathway.wayPoints[i - 1], _pathway.wayPoints[i], NavMesh.AllAreas, _pathway.Path);
		}
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
						GenerateNavMeshPath();
						InternalEditorUtility.RepaintAllViews();
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
				_pathway.DisplayPolls = true;
				_pathway.Path.ClearCorners();
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
				if (GUILayout.Button("Display Polls"))
				{
					_pathway.DisplayPolls = true;
					InternalEditorUtility.RepaintAllViews();
				}
			}
		}
	}

}
