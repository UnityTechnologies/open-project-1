using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClickToPlaceHelper))]
public class ClickToPlaceHelperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ClickToPlaceHelper myTarget = target as ClickToPlaceHelper;

		if (GUILayout.Button("Place at Mouse cursor"))
		{
			myTarget.SetSpawnLocationAtCursor();
		}
	}
}
