using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;
using static UnityEditor.EditorJsonUtility;
using static UnityEngine.Application;

namespace UOP1.EditorTools.Replacer
{
	internal class ReplacePrefabSearchPopup : EditorWindow
	{
		private const float previewHeight = 128;

		private class ViewState : ScriptableObject
		{
			public TreeViewState treeViewState = new TreeViewState();
		}

		private static ReplacePrefabSearchPopup window;
		private static Styles styles;

		private static Event evt => Event.current;
		private static string assetPath => Path.Combine(dataPath.Remove(dataPath.Length - 7, 7), "Library", "ReplacePrefabTreeState.asset");

		private bool hasSelection => tree.state.selectedIDs.Count > 0;
		private int selectedId => tree.state.selectedIDs[0];
		private GameObject instance => EditorUtility.InstanceIDToObject(selectedId) as GameObject;

		private SearchField searchField;
		private ReplacePrefabTreeView tree;
		private ViewState viewState;

		private Vector2 startPos;
		private Vector2 startSize;

		private GameObjectPreview selectionPreview = new GameObjectPreview();

		public static void Show(Rect rect)
		{
			var windows = Resources.FindObjectsOfTypeAll<ReplacePrefabSearchPopup>();
			window = windows.Length != 0 ? windows[0] : CreateInstance<ReplacePrefabSearchPopup>();

			window.Init();

			window.startPos = rect.position;
			window.startSize = rect.size;

			window.position = new Rect(rect.position, rect.size);
			window.SetCorrectSize();
			//window.ShowAsDropDown(new Rect(position, Vector2.zero), size);

			// This type of window supports resizing, but is also persistent, so we need to close it manually
			window.ShowPopup();

			//onSelectEntry += _ => window.Close();
		}

		private void Init()
		{
			viewState = CreateInstance<ViewState>();

			if (File.Exists(assetPath))
				FromJsonOverwrite(File.ReadAllText(assetPath), viewState);

			tree = new ReplacePrefabTreeView(viewState.treeViewState);
			tree.onSelectEntry += OnSelectEntry;

			AssetPreview.SetPreviewTextureCacheSize(tree.RowsCount);

			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += tree.SetFocusAndEnsureSelectedItem;
			searchField.SetFocus();
		}

		private void OnSelectEntry(GameObject prefab)
		{
			ReplaceTool.ReplaceSelectedObjects(Selection.gameObjects, prefab);
		}

		private void OnEnable()
		{
			Init();
		}

		private void OnDisable()
		{
			tree.Cleanup();
		}

		public new void Close()
		{
			SaveState();
			base.Close();
		}

		private void SaveState()
		{
			File.WriteAllText(assetPath, ToJson(viewState));
		}

		private void OnGUI()
		{
			if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
			{
				if (tree.hasSearch)
					tree.searchString = "";
				else
					Close();
			}

			if (focusedWindow != this)
				Close();

			if (styles == null)
				styles = new Styles();

			DoToolbar();
			DoTreeView();
			DoSelectionPreview();
		}

		void DoToolbar()
		{
			tree.searchString = searchField.OnToolbarGUI(tree.searchString);
			GUILayout.Space(-2);

			GUILayout.Label("Replace With...", styles.headerLabel);
		}

		void DoTreeView()
		{
			var rect = GUILayoutUtility.GetRect(0, 10000, 0, 10000);
			rect.x += 2;
			rect.width -= 4;

			rect.y += 2;
			rect.height -= 4;

			tree.OnGUI(rect);
		}

		void DoSelectionPreview()
		{
			SetCorrectSize();

			if (hasSelection && tree.IsRenderable(selectedId))
			{
				var previewRect = GUILayoutUtility.GetRect(position.width, previewHeight);

				selectionPreview.CreatePreviewForTarget(instance);

				selectionPreview.RenderInteractivePreview(previewRect);

				selectionPreview.DrawPreviewTexture(previewRect);
			}
		}

		private void SetCorrectSize()
		{
			if (hasSelection && tree.IsRenderable(selectedId))
				SetSize(startSize.x, startSize.y + previewHeight);
			else
				SetSize(startSize.x, startSize.y);
		}

		private void SetSize(float width, float height)
		{
			position = new Rect(startPos.x, startPos.y, width, height);
		}

		private class Styles
		{
			public GUIStyle header = new GUIStyle("AC BoldHeader")
			{
				fixedHeight = 0,
				stretchWidth = true,
				margin = new RectOffset(0, 0, 0, 0)
			};

			public GUIStyle headerLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 11,
				fontStyle = FontStyle.Bold
			};
		}
	}
}
