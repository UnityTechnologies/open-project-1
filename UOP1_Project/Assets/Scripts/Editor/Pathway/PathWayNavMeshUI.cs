using UnityEngine;
using UnityEditorInternal;

public class PathWayNavMeshUI
{
	private bool _toggled;
	private Pathway _pathway;
	private PathwayNavMesh _pathwayNavMesh;

	public PathWayNavMeshUI(Pathway pathway)
	{
		_pathway = pathway;
		_pathwayNavMesh = new PathwayNavMesh(pathway);
		_pathway.DisplayPolls = false;
		_pathway.TogglePathDisplay = false;
		_toggled = false;
	}

	public void OnInspectorGUI()
	{
		if (_toggled == false)
		{
			NavMeshPathButton();
		}
		else
		{
			HandlesPathButton();

			if (_pathway.Waypoints.Count > 1)
			{
				PollsButtons();
			}
			else
			{
				_toggled = false;
				_pathway.TogglePathDisplay = false;
				_pathway.DisplayPolls = false;
			}
		}
	}

	private void NavMeshPathButton()
	{
		if (_toggled = GUILayout.Button("NavMesh Path"))
		{
			if (_pathwayNavMesh.PollsNavMesh())
			{
				if (_pathway.Waypoints.Count > 1)
				{
					if (_pathwayNavMesh.GenerateNavMeshPath())
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
					Debug.LogError("NavMesh need more than one point to calculate the path");
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

	private void HandlesPathButton()
	{
		if (GUILayout.Button("Handles Path"))
		{
			_toggled = false;
			_pathway.TogglePathDisplay = false;
			_pathway.DisplayPolls = false;
			_pathway.Path.Clear();
			InternalEditorUtility.RepaintAllViews();
		}
	}

	private void PollsButtons()
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
				_pathwayNavMesh.PollsNavMesh();
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

}
