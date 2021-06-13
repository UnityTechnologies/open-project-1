using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ViewCreatorCamera))]
public class ViewCreatorCameraRelease: Editor
{
	private ViewCreatorCamera viewCamera => target as ViewCreatorCamera;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("capture view"))
		{
			viewCamera.captureView();
		}
	}
}
