using UnityEngine;
using UnityEditorInternal;
using UnityEditor;


public class PathWayNavMeshUI
{
	private Pathway _pathway;
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayPolls;
	private SerializedProperty _togglePathDisplay;

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

	public PathWayNavMeshUI(SerializedObject serializedObject, Pathway pathway)
	{
		_pathway = pathway;
		_displayPolls = serializedObject.FindProperty("DisplayPolls");
		_togglePathDisplay = serializedObject.FindProperty("TogglePathDisplay");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject, pathway);
		GeneratePath();
	}

	public void OnInspectorGUI()
	{
		if (!TogglePathDisplay )
		{
			if (GUILayout.Button("NavMesh Path"))
			{
				GeneratePath();
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				TogglePathDisplay = false;
				DisplayPolls = false;
				InternalEditorUtility.RepaintAllViews();
			}
		}
	}

	public void GeneratePath() {

		DisplayPolls = !_pathwayNavMesh.hasNavMesh();

		if (!DisplayPolls)
		{
			if (_pathway.Waypoints.Count > 1)
			{
				if (_pathwayNavMesh.GenerateNavMeshPath())
				{
					TogglePathDisplay = true;
					InternalEditorUtility.RepaintAllViews();
				}
			}
		}
		else
		{
			InternalEditorUtility.RepaintAllViews();
		}
	}

	public void ProbeUpdate()
	{
		if(TogglePathDisplay)
			DisplayPolls=!_pathwayNavMesh.hasNavMesh();
	}

	public void PathUpdate()
	{
		if (TogglePathDisplay && _pathwayNavMesh.hasNavMesh())
		{
			_pathwayNavMesh.GenerateNavMeshPath();
		}
	}

	public void RealTime()
	{
		if (_pathway.RealTimeEnabled && GUI.changed)
		{
				GeneratePath();
		}
	}

}
