using UnityEngine;
using UnityEditorInternal;
using UnityEditor;


public class PathWayNavMeshUI
{
	private Pathway _pathway;
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayProbes;
	private SerializedProperty _toggledNavMeshDisplay;

	public PathWayNavMeshUI(SerializedObject serializedObject, Pathway pathway)
	{
		_pathway = pathway;
		_displayProbes = serializedObject.FindProperty("DisplayProbes");
		_toggledNavMeshDisplay = serializedObject.FindProperty("ToggledNavMeshDisplay");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject, pathway);
		GeneratePath();
	}

	public void OnInspectorGUI()
	{
		if (!_toggledNavMeshDisplay.boolValue)
		{
			if (GUILayout.Button("NavMesh Path"))
			{
				GeneratePath();
				_displayProbes.boolValue = !_toggledNavMeshDisplay.boolValue;
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				_toggledNavMeshDisplay.boolValue = false;
				_displayProbes.boolValue = false;
			}
		}
	}

	public void GeneratePath() {

		_displayProbes.boolValue = !_pathwayNavMesh.hasNavMesh();

		if (!_displayProbes.boolValue)
		{
			_toggledNavMeshDisplay.boolValue = _pathwayNavMesh.GenerateNavMeshPath();
		}
	}


	public void PathUpdate()
	{
		if (_pathway.RealTimeEnabled)
		{
			_displayProbes.boolValue = !_pathwayNavMesh.hasNavMesh();
			
			if (!_displayProbes.boolValue && !GUI.changed)
			{
				_displayProbes.boolValue = !_pathwayNavMesh.GenerateNavMeshPath();
			}
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
