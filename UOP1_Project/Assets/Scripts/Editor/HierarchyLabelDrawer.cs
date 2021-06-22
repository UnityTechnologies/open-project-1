using UnityEditor;
using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Draws all hierarchy labels in the hierarchy window.
	/// </summary>
	[InitializeOnLoad]
	internal sealed class HierarchyLabelDrawer
	{
		static HierarchyLabelDrawer()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
		}

		private static GUIStyle _style;

		internal static GUIStyle Style
		{
			get
			{
				if (_style == null)
				{
					_style = new GUIStyle("PreOverlayLabel")
					{
						richText = true,
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Bold
					};
					_style.normal.textColor = Color.white;
				}

				return _style;
			}
		}

		[InitializeOnLoadMethod]
		static void OnPackageLoadedInEditor()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
		}

		private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
		{
			GameObject instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

			if (instance == null)
			{
				return;
			}

			HierarchyLabel label = instance.GetComponent<HierarchyLabel>();

			if (label != null)
			{
				EditorGUI.DrawRect(selectionRect, label.BackgroundColor);
				GUI.contentColor = label.TextColor;
				GUI.Label(selectionRect, label.Text, Style);
				GUI.contentColor = Color.white;
			}
		}
	}
}
