using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UOP1.EditorTools.Replacer
{
	internal class ReplacePrefabTreeView : TreeView
	{
		private static Texture2D folderIcon = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;

		public Action<GameObject> onSelectEntry;

		private List<TreeViewItem> rows = new List<TreeViewItem>();
		private HashSet<string> paths = new HashSet<string>();

		private int selectedId;

		public ReplacePrefabTreeView(TreeViewState state) : base(state)
		{
			Reload();
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		protected override void DoubleClickedItem(int id)
		{
			var instance = EditorUtility.InstanceIDToObject(id);

			if (instance is GameObject gameObject)
				onSelectEntry(gameObject);
		}

		protected override void KeyEvent()
		{
			var key = Event.current.keyCode;
			if (key == KeyCode.KeypadEnter || key == KeyCode.Return)
				DoubleClickedItem(selectedId);
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			selectedId = selectedIds[0];
		}

		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem(0, -1);
			rows.Clear();
			paths.Clear();

			paths.Add("Prefabs");

			var prefabs = new TreeViewItem(1, 0, "Prefabs");
			rows.Add(prefabs);

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

		private void AddFoldersItems(string[] splits)
		{
			for (int i = 1; i < splits.Length - 1; i++)
			{
				var split = splits[i];

				if (!paths.Contains(split))
				{
					rows.Add(new TreeViewItem(split.GetHashCode(), i - 1, split) { icon = folderIcon });
					paths.Add(split);
				}
			}
		}

		private void AddPrefabItem(GameObject asset, int depth)
		{
			var content = new GUIContent(EditorGUIUtility.ObjectContent(asset, asset.GetType()));

			rows.Add(new TreeViewItem(asset.GetInstanceID(), depth, content.text)
			{
				icon = content.image as Texture2D
			});
		}
	}
}
