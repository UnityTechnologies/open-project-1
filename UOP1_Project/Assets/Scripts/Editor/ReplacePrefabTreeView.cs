using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UOP1.EditorTools.Replacer
{
	internal class ReplacePrefabTreeView : TreeView
	{
		private static Texture2D prefabOnIcon = EditorGUIUtility.IconContent("Prefab On Icon").image as Texture2D;
		private static Texture2D prefabVariantOnIcon = EditorGUIUtility.IconContent("PrefabVariant On Icon").image as Texture2D;
		private static Texture2D folderIcon = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;

		private static GUIStyle whiteLabel;

		public int RowsCount => rows.Count;

		public Action<GameObject> onSelectEntry;

		private List<TreeViewItem> rows = new List<TreeViewItem>();
		private HashSet<string> paths = new HashSet<string>();

		private Dictionary<int, RenderTexture> previewCache = new Dictionary<int, RenderTexture>();
		private HashSet<int> renderableItems = new HashSet<int>();

		private GameObjectPreview itemPreview = new GameObjectPreview();
		private GUIContent itemContent = new GUIContent();

		private int selectedId;

		public ReplacePrefabTreeView(TreeViewState state) : base(state)
		{
			Reload();
		}

		public void Cleanup()
		{
			foreach (var texture in previewCache.Values)
				Object.DestroyImmediate(texture);
		}

		public bool IsRenderable(int id)
		{
			return renderableItems.Contains(id);
		}

		private void CachePreview(int itemId)
		{
			var copy = new RenderTexture(itemPreview.outputTexture);
			var previous = RenderTexture.active;
			Graphics.Blit(itemPreview.outputTexture, copy);
			RenderTexture.active = previous;
			previewCache.Add(itemId, copy);
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		private bool IsPrefabAsset(int id, out GameObject prefab)
		{
			var obj = EditorUtility.InstanceIDToObject(id);

			if (obj is GameObject go)
			{
				prefab = go;
				return true;
			}

			prefab = null;
			return false;
		}

		protected override void DoubleClickedItem(int id)
		{
			if (IsPrefabAsset(id, out var prefab))
				onSelectEntry(prefab);
		}

		protected override void KeyEvent()
		{
			var key = Event.current.keyCode;
			if (key == KeyCode.KeypadEnter || key == KeyCode.Return)
				DoubleClickedItem(selectedId);
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			if (selectedIds.Count > 0)
				selectedId = selectedIds[0];
		}

		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem(0, -1);
			rows.Clear();
			paths.Clear();

			foreach (var guid in AssetDatabase.FindAssets("t:Prefab"))
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var splits = path.Split('/');
				var depth = splits.Length - 2;

				if (splits[0] != "Assets")
					break;

				var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				AddFoldersItems(splits);
				AddPrefabItem(asset, depth);
			}

			SetupParentsAndChildrenFromDepths(root, rows);

			return root;
		}

		protected override float GetCustomRowHeight(int row, TreeViewItem item)
		{
			// Hide folders during search
			if (!IsPrefabAsset(item.id, out _) && hasSearch)
				return 0;

			return 20;
		}

		public override void OnGUI(Rect rect)
		{
			if (whiteLabel == null)
				whiteLabel = new GUIStyle(EditorStyles.label) { normal = { textColor = EditorStyles.whiteLabel.normal.textColor }};

			base.OnGUI(rect);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			var rect = args.rowRect;
			var item = args.item;

			var isRenderable = IsRenderable(item.id);
			var isSelected = IsSelected(item.id);
			var isFocused = HasFocus() && isSelected;
			var isPrefab = IsPrefabAsset(item.id, out var prefab);

			if (!isPrefab && hasSearch)
				return;

			var labelStyle = isFocused ? whiteLabel : EditorStyles.label;
			var contentIndent = GetContentIndent (item);

			customFoldoutYOffset = 2;
			itemContent.text = item.displayName;

			rect.x += contentIndent;
			rect.width -= contentIndent;

			var iconRect = new Rect(rect) { width = 20 };

			if (isPrefab)
			{
				var type = PrefabUtility.GetPrefabAssetType(prefab);
				var onIcon = type == PrefabAssetType.Regular ? prefabOnIcon : prefabVariantOnIcon;

				var labelRect = new Rect(rect);

				if (isRenderable)
				{
					var previewRect = new Rect(rect) { width = 32, height = 32 };

					if (!previewCache.TryGetValue(item.id, out var previewTexture))
					{
						itemPreview.CreatePreviewForTarget(prefab);
						itemPreview.RenderInteractivePreview(previewRect);

						if (itemPreview.outputTexture)
							CachePreview(item.id);
					}

					if (!previewTexture)
						Repaint();

					GUI.DrawTexture(iconRect, previewTexture, ScaleMode.ScaleAndCrop);

					labelRect.x += iconRect.width;
					labelRect.width -= iconRect.width + 24;

					GUI.Label(labelRect, args.label, labelStyle);

					if (isSelected)
					{
						var prefabIconRect = new Rect(iconRect) { x = rect.xMax - 24 };
						GUI.Label(prefabIconRect, isFocused ? onIcon : item.icon);
					}
				}
				else
				{
					itemContent.image = isSelected ? onIcon : item.icon;
					GUI.Label(rect, itemContent, labelStyle);
				}
			}
			else
			{
				itemContent.image = folderIcon;
				GUI.Label(rect, itemContent, labelStyle);
			}
		}

		private void AddFoldersItems(string[] splits)
		{
			for (int i = 1; i < splits.Length - 1; i++)
			{
				var split = splits[i];

				if (!paths.Contains(split))
				{
					rows.Add(new TreeViewItem(split.GetHashCode(), i - 1, " " + split) { icon = folderIcon });
					paths.Add(split);
				}
			}
		}

		private void AddPrefabItem(GameObject asset, int depth)
		{
			var id = asset.GetInstanceID();
			var content = new GUIContent(EditorGUIUtility.ObjectContent(asset, asset.GetType()));

			if (GameObjectPreview.HasRenderableParts(asset))
				renderableItems.Add(id);

			rows.Add(new TreeViewItem(id, depth, content.text)
			{
				icon = content.image as Texture2D
			});
		}
	}
}
