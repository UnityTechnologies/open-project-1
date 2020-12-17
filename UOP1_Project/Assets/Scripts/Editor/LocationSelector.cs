
using UnityEditor;
using UnityEngine;

public class LocationSelector : EditorWindow
{
	[MenuItem("Window/Designer Tools/Location Selector")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(LocationSelector));
	}
	void OnGUI()
	{
		Editor editor = Editor.CreateEditor(SelectorSurrogateSO.Instance);
		editor.DrawDefaultInspector();
		EditorGUILayout.Space();
		if (GUILayout.Button("Load Location (Only In Playmode)"))
		{
			if (Application.isPlaying)
			{
				SelectorSurrogateSO.Instance.LoadChannel.RaiseEvent(SelectorSurrogateSO.Instance.Point, false);
			}
		}
	}

}
