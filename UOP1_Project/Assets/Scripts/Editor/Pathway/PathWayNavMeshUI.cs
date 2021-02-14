using UnityEngine;
using UnityEditorInternal;
using UnityEditor;

public class PathWayNavMeshUI
{
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayPolls;
	private SerializedProperty _togglePathDisplay;
	private SerializedProperty _waypoints;

	private bool DisplayPolls
	{
		get => _displayPolls.boolValue;
		set => _displayPolls.boolValue = value;
	}

	private bool TogglePathDisplay
	{
		get => _togglePathDisplay.boolValue;
		set => _togglePathDisplay.boolValue = value;
	}

	public PathWayNavMeshUI(SerializedObject serializedObject)
	{
		_displayPolls = serializedObject.FindProperty("DisplayPolls");
		_togglePathDisplay = serializedObject.FindProperty("TogglePathDisplay");
		_waypoints = serializedObject.FindProperty("Waypoints");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject);
	}

	public void OnInspectorGUI()
	{
		if (!TogglePathDisplay )
		{
			NavMeshPathButton();
		}
		else
		{
			HandlesPathButton();

			if (_waypoints.arraySize > 1)
			{
				PollsButtons();
			}
			else
			{
				TogglePathDisplay = false;
				DisplayPolls = false;
			}
		}
	}

	private void NavMeshPathButton()
	{
		if ( GUILayout.Button("NavMesh Path"))
		{
			if (_pathwayNavMesh.PollsNavMesh())
			{
				if (_waypoints.arraySize > 1)
				{
					if (_pathwayNavMesh.GenerateNavMeshPath())
					{
						TogglePathDisplay = true;
						InternalEditorUtility.RepaintAllViews();
					}
					else
					{
						TogglePathDisplay = false;
					}
				}
				else
				{
					Debug.LogError("NavMesh need more than one point to calculate the path");
					TogglePathDisplay = false;
				}
			}
			else
			{
				TogglePathDisplay = false;
				DisplayPolls = true;
				InternalEditorUtility.RepaintAllViews();
			}
		}
	}

	private void HandlesPathButton()
	{
		if (GUILayout.Button("Handles Path"))
		{
			TogglePathDisplay = false;
			DisplayPolls = false;
			InternalEditorUtility.RepaintAllViews();
		}
	}

	private void PollsButtons()
	{
		if (DisplayPolls)
		{
			if (GUILayout.Button("Hide Polls"))
			{
				DisplayPolls = false;
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
				DisplayPolls = true;
				InternalEditorUtility.RepaintAllViews();
			}
		}
	}

}
