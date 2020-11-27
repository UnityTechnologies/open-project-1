using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UOP1.EditorTools.Replacer
{
	internal class ReplaceContextMenu
	{
		private static Type hierarchyType;

		private static EditorWindow focusedWindow;
		private static EditorWindow focusedHierarchy;
		private static IMGUIContainer hierarchyGUI;

		private static Vector2 mousePosition;
		private static Dictionary<int, Rect> hierarchyRects = new Dictionary<int, Rect>();

		[InitializeOnLoadMethod]
		private static void OnInitialize()
		{
			hierarchyType = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");

			EditorApplication.update += TrackFocusedHierarchy;

			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
			EditorApplication.hierarchyChanged += hierarchyRects.Clear;
		}

		private static void TrackFocusedHierarchy()
		{
			if (focusedWindow != EditorWindow.focusedWindow)
			{
				focusedWindow = EditorWindow.focusedWindow;

				if (focusedWindow.GetType() == hierarchyType)
				{
					focusedHierarchy = focusedWindow;

					if (hierarchyGUI != null)
						hierarchyGUI.onGUIHandler -= OnFocusedHierarchyGUI;

					hierarchyGUI = focusedWindow.rootVisualElement.parent.Query<IMGUIContainer>();
					hierarchyGUI.onGUIHandler += OnFocusedHierarchyGUI;
				}
			}
		}

		private static void OnFocusedHierarchyGUI()
		{
			// As Event.current is null during context-menu callback, we need to track mouse position on hierarchy GUI
			mousePosition = Event.current.mousePosition;
		}

		[MenuItem("GameObject/Replace", priority = 0)]
		private static void ReplaceSelection()
		{
			hierarchyRects.TryGetValue(Selection.activeInstanceID, out var rect);

			var size = new Vector2(240, 320);
			var position = focusedHierarchy.position.position;

			position.x = mousePosition.x;
			position.y += rect.y + 20;

			ReplacePrefabSearchPopup.Show(rect, position, size, prefab =>
			{
				ReplaceTool.ReplaceSelectedObjects(Selection.gameObjects, prefab);
			});
		}

		private static void OnHierarchyItemGUI(int instanceId, Rect selectionRect)
		{
			if (!hierarchyRects.ContainsKey(instanceId))
			{
				hierarchyRects.Add(instanceId, selectionRect);
			}
			else
			{
				hierarchyRects[instanceId] = selectionRect;
			}
		}
	}
}
