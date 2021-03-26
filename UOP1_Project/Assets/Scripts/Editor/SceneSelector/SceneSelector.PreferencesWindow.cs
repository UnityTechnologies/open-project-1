using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using SceneSelectorInternal;

public partial class SceneSelector : EditorWindow
{
	private class PreferencesWindow : EditorWindow
	{
		private class Styles
		{
			public GUIStyle itemBorder;
			public GUIStyle buttonVisibilityOn;
			public GUIStyle buttonVisibilityOff;
		}

		private const string kWindowCaption = "Scene Selector Preferences";
		private const float kHeaderHeight = 0.0f;
		private const float kItemHeight = 24.0f;
		private const float kVisibilityButtonSize = 16.0f;

		public static float kColorMarkerFieldSize = Mathf.Ceil(Helper.kColorMarkerNormalSize * 1.41f + 8.0f);
		private static readonly Color kItemBorderColor = new Color(1.0f, 1.0f, 1.0f, 0.16f);

		private SceneSelector _owner;
		private ColorSelectorWindow _colorSelectorWindow;
		private ReorderableList _itemsReorderableList;
		private Styles _styles;
		private Vector2 _windowScrollPosition;

		private List<Item> items => _owner._storage.items;

		public static PreferencesWindow Open(SceneSelector owner)
		{
			var window = GetWindow<PreferencesWindow>(true, kWindowCaption, true);
			window.Init(owner);
			return window;
		}

		private void OnEnable()
		{
			wantsMouseMove = true;
		}

		private void OnDisable()
		{
			_owner.SaveStorage();
			if (_colorSelectorWindow != null)
				_colorSelectorWindow.Close();
		}

		private void OnGUI()
		{
			EnsureStyles();
			Helper.RepaintOnMouseMove(this);
			DrawWindow();
		}

		public void RepaintAll()
		{
			RepaintOwner();
			Repaint();
		}

		private void Init(SceneSelector owner)
		{
			_owner = owner;
			CreateReorderableList();
		}

		private void CreateReorderableList()
		{
			_itemsReorderableList = new ReorderableList(items, typeof(Item), true, true, false, false);
			_itemsReorderableList.drawElementCallback = DrawItem;
			_itemsReorderableList.drawElementBackgroundCallback = DrawItemBackground;
			_itemsReorderableList.onReorderCallback = OnReorder;
			_itemsReorderableList.headerHeight = kHeaderHeight;
			_itemsReorderableList.elementHeight = kItemHeight;
		}

		private void DrawWindow()
		{
			using (var scrollScope = new EditorGUILayout.ScrollViewScope(_windowScrollPosition))
			{
				GUILayout.Space(4.0f);
				_itemsReorderableList.DoLayoutList();
				_windowScrollPosition = scrollScope.scrollPosition;
			}
		}

		private void DrawItem(Rect rect, int index, bool isActive, bool isFocused)
		{
			var item = items[index];
			var gameScene = item.gameSceneSO;
			if (gameScene != null)
			{
				var colorMarkerRect = rect;
				colorMarkerRect.width = colorMarkerRect.height;

				if (Helper.DrawColorMarker(colorMarkerRect, item.color, true, true))
				{
					var colorSelectorRect = GUIUtility.GUIToScreenRect(colorMarkerRect);
					_colorSelectorWindow = ColorSelectorWindow.Open(colorSelectorRect, this, item);
				}

				var itemLabelRect = rect;
				itemLabelRect.x += colorMarkerRect.width;
				itemLabelRect.width -= kVisibilityButtonSize + colorMarkerRect.width;

				GUI.Label(itemLabelRect, gameScene.name);

				var visibilityButtonRect = new Rect(rect);
				visibilityButtonRect.width = kVisibilityButtonSize;
				visibilityButtonRect.height = kVisibilityButtonSize;
				visibilityButtonRect.x = itemLabelRect.x + itemLabelRect.width;
				visibilityButtonRect.y += (rect.height - visibilityButtonRect.height) * 0.5f;

				var visibilityStyle = item.isVisible
				? _styles.buttonVisibilityOn
				: _styles.buttonVisibilityOff;

				if (GUI.Button(visibilityButtonRect, GUIContent.none, visibilityStyle))
				{
					item.isVisible = !item.isVisible;
					RepaintOwner();
				}
			}
		}

		private void DrawItemBackground(Rect rect, int index, bool isActive, bool isFocused)
		{
			ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, true);
			using (Helper.ReplaceColor.With(kItemBorderColor))
			{
				GUI.Box(rect, GUIContent.none, _styles.itemBorder);
			}
		}

		private void OnReorder(ReorderableList _)
		{
			RepaintOwner();
		}

		private void RepaintOwner()
		{
			_owner.Repaint();
		}

		private void EnsureStyles()
		{
			if (_styles == null)
			{
				_styles = new Styles();

				_styles.itemBorder = new GUIStyle(GUI.skin.GetStyle("HelpBox"));

				_styles.buttonVisibilityOn = new GUIStyle(GUI.skin.label);
				_styles.buttonVisibilityOn.padding = new RectOffset(0, 0, 0, 0);
				_styles.buttonVisibilityOn.normal.background = EditorGUIUtility.FindTexture("d_scenevis_visible");
				_styles.buttonVisibilityOn.hover.background = EditorGUIUtility.FindTexture("d_scenevis_visible_hover");

				_styles.buttonVisibilityOff = new GUIStyle(GUI.skin.label);
				_styles.buttonVisibilityOff.padding = new RectOffset(0, 0, 0, 0);
				_styles.buttonVisibilityOff.normal.background = EditorGUIUtility.FindTexture("d_scenevis_hidden");
				_styles.buttonVisibilityOff.hover.background = EditorGUIUtility.FindTexture("d_scenevis_hidden_hover");
			}
		}
	}
}
