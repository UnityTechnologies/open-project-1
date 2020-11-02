using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnLocation))]
public class SpawnLocationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SpawnLocation myTarget = target as SpawnLocation;

		if (GUILayout.Button("Place at Cursor"))
		{
			myTarget.SetSpawnLocationAtCursor();
		}
	}
}