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
		private PrefabSelectionTreeView tree;
		private ViewState viewState;

		private Vector2 startPos;
		private Vector2 startSize;
		private Vector2 lastSize;

		private GameObjectPreview selectionPreview = new GameObjectPreview();

		public static void Show(Rect rect)
		{
			var windows = Resources.FindObjectsOfTypeAll<ReplacePrefabSearchPopup>();
			window = windows.Length != 0 ? windows[0] : CreateInstance<ReplacePrefabSearchPopup>();

			window.Init();

			window.startPos = rect.position;
			window.startSize = rect.size;

			window.position = new Rect(rect.position, rect.size);
			// Need to predict start window size to avoid trash frame
			window.SetInitialSize();

			// This type of window supports resizing, but is also persistent, so we need to close it manually
			window.ShowPopup();

			//onSelectEntry += _ => window.Close();
		}

		private void Init()
		{
			viewState = CreateInstance<ViewState>();

			if (File.Exists(assetPath))
				FromJsonOverwrite(File.ReadAllText(assetPath), viewState);

			tree = new PrefabSelectionTreeView(viewState.treeViewState);
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
			if (hasSelection && tree.IsRenderable(selectedId))
			{
				SetSize(startSize.x, startSize.y + previewHeight);
				var previewRect = GUILayoutUtility.GetRect(position.width, previewHeight);

				selectionPreview.CreatePreviewForTarget(instance);
				selectionPreview.RenderInteractivePreview(previewRect);

				selectionPreview.DrawPreviewTexture(previewRect);
			}
			else
			{
				SetSize(startSize.x, startSize.y);
			}
		}

		private void SetInitialSize()
		{
			if (hasSelection && tree.IsRenderable(selectedId))
				SetSize(startSize.x, startSize.y + previewHeight);
			else
				SetSize(startSize.x, startSize.y);
		}

		private void SetSize(float width, float height)
		{
			var newSize = new Vector2(width, height);
			if (newSize != lastSize)
			{
				lastSize = newSize;
				position = new Rect(position.x, position.y, width, height);
			}
		}

		private class Styles
		{
			public GUIStyle headerLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 11,
				fontStyle = FontStyle.Bold
			};
		}
	}
}
