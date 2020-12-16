
using UnityEditor;
using UnityEngine;

public class SceneSelector : EditorWindow
{
	[MenuItem("Window/Designer Tools/Scene Selector")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SceneSelector));
	}
	void OnGUI()
	{
		Editor editor = Editor.CreateEditor(SelectorSurrogateSO.Instance);
		editor.DrawDefaultInspector();
		EditorGUILayout.Space();
		if (GUILayout.Button("Load Scene (Only In Playmode)"))
		{
			if (Application.isPlaying)
			{
				SelectorSurrogateSO.Instance.LoadChannel.RaiseEvent(SelectorSurrogateSO.Instance.Location, false);
			}
		}
	}

}
