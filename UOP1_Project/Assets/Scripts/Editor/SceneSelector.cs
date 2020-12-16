using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelector : EditorWindow, IHasCustomMenu
{
	private class ColorSelectorWindow : EditorWindow
	{
		private static readonly float kCellSize = PreferencesWindow.kColorMarkerFieldSize * 2.0f;
		private static readonly Vector2Int kCount = new Vector2Int(5, 5);

		private Color[,] _colors;
		private Item _item;

		public static ColorSelectorWindow Open(Rect rect, Item item)
		{
			var window = CreateInstance<ColorSelectorWindow>();
			window.Init(rect, item);
			return window;
		}

		private void Init(Rect rect, Item item)
		{
			var size = (Vector2)kCount * kCellSize;
			ShowAsDropDown(rect, size);
			_item = item;
		}

		private void OnEnable()
		{
			wantsMouseMove = true;
			InitColors();
		}

		private void OnGUI()
		{
			RepaintOnMouseMove(this);
			DrawMarkers();
		}

		private void DrawMarkers()
		{
			var size = new Vector2(kCellSize, kCellSize);
			for (int x = 0; x < kCount.x; ++x)
			{
				for (int y = 0; y < kCount.y; ++y)
				{
					var color = _colors[x, y];
					var position = size * new Vector2(x, y);
					var rect = new Rect(position, size);
					if (DrawColorMarker(rect, color, true, true))
					{
						_item.color = color;
						Close();
					}
				}
			}
		}

		private void InitColors()
		{
			var count = kCount.x * kCount.y;
			_colors = new Color[kCount.x, kCount.y];
			for (int x = 0; x < kCount.x; ++x)
			{
				var h = x * kCount.y;
				for (int y = 0; y < kCount.y; ++y)
				{
					float hue = (float)(h + y) / count;
					_colors[x, y] = Color.HSVToRGB(hue, 1.0f, 1.0f);
				}
			}
		}
	}

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

		public static float kColorMarkerFieldSize = Mathf.Ceil(kColorMarkerNormalSize * 1.41f + 8.0f);
		private static readonly Color kItemBorderColor = new Color(1.0f, 1.0f, 1.0f, 0.16f);

		private SceneSelector _owner;
		private ColorSelectorWindow _colorSelectorWindow;
		private ReorderableList _itemsReorderableList;
		private Styles _styles;
		private Vector2 _windowScrollPosition;

		private List<Item> items => _owner._storage.items;
		private Dictionary<int, Item> itemsMap => _owner._storage.itemsMap;

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

		private void Init(SceneSelector owner)
		{
			_owner = owner;
			CreateReorderableList();
		}

		private void OnGUI()
		{
			EnsureStyles();
			RepaintOnMouseMove(this);
			DrawWindow();
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
			var gameScene = item.gameScene;
			if (gameScene != null)
			{
				var colorMarkerRect = rect;
				colorMarkerRect.width = colorMarkerRect.height;

				if (DrawColorMarker(colorMarkerRect, item.color, true, true))
				{
					_colorSelectorWindow = ColorSelectorWindow.Open(GUIUtility.GUIToScreenRect(colorMarkerRect), item);
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
			using (ReplaceColor.With(kItemBorderColor))
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

				_styles.itemBorder = GUI.skin.GetStyle("HelpBox");

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

	private struct ReplaceColor : IDisposable
	{
		public static ReplaceColor With(Color color) => new ReplaceColor(color);

		private Color _oldColor;

		private ReplaceColor(Color color)
		{
			_oldColor = GUI.color;
			GUI.color = color;
		}

		void IDisposable.Dispose() => GUI.color = _oldColor;
	}

	[Serializable]
	private class Item
	{
		public int id;
		public int order = int.MaxValue;
		public bool isVisible = true;
		public Color color = Color.red;

		[NonSerialized]
		public GameSceneSO gameScene;
	}

	private class Styles
	{
		public GUIStyle item;
	}

	private class Textures
	{
		public Texture2D visibilityOn;
		public Texture2D visibilityOff;
	}

	[Serializable]
	private class Storage : ISerializationCallbackReceiver
	{
		[SerializeField]
		public List<Item> items = new List<Item>();

		[NonSerialized]
		public Dictionary<int, Item> itemsMap = new Dictionary<int, Item>();

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			for (int i = 0, count = items.Count; i < count; ++i)
			{
				var item = items[i];
				item.order = i;
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			items.OrderBy(x => x.order);
			foreach (var item in items)
			{
				itemsMap.Add(item.id, item);
			}
		}
	}

	private const string kPreferencesKey = "uop1.SceneSelector.Preferences";
	private const float kColorMarkerNormalSize = 4.0f;
	private const float kColorMarkerHoveredSize = 6.0f;
	private const int kItemContentLeftPadding = 32;
	private static readonly GUIContent kOpenPreferencesItemContent = new GUIContent("Open Preferences");
	private static readonly Color kColorMarkerLightTint = new Color(1.0f, 1.0f, 1.0f, 0.32f);
	private static readonly Color kColorMarkerDarkTint = Color.gray;

	private Styles _styles;
	private Storage _storage;
	private PreferencesWindow _preferencesWindow;
	private Vector2 _windowScrollPosition;
	private bool _hasEmptyItems;

	private List<Item> items => _storage.items;
	private Dictionary<int, Item> itemsMap => _storage.itemsMap;

	[MenuItem("Window/Scene Selector", false, 100)]
	private static void Open()
	{
		GetWindow<SceneSelector>();
	}

	private void OnEnable()
	{
		wantsMouseMove = true;
		LoadStorage();
		PopulateItems();
		GameSceneSO.onEnabled += OnGameSceneSOCreated;
	}

	private void OnDisable()
	{
		if (_preferencesWindow != null)
			_preferencesWindow.Close();
		SaveStorage();
		GameSceneSO.onEnabled -= OnGameSceneSOCreated;
	}

	private void OnGUI()
	{
		EnsureStyles();
		RepaintOnMouseMove(this);
		RemoveEmptyItemsIfRequired();
		DrawWindow();
	}

	private void DrawWindow()
	{
		using (var scrollScope = new EditorGUILayout.ScrollViewScope(_windowScrollPosition))
		{
			GUILayout.Space(4.0f);
			DrawItems();
			_windowScrollPosition = scrollScope.scrollPosition;
		}
	}

	private void DrawItems()
	{
		foreach (var item in items)
		{
			DrawItem(item);
		}
	}

	private void DrawItem(Item item)
	{
		if (item.isVisible)
		{
			var gameScene = item.gameScene;
			if (gameScene != null)
			{
				if (GUILayout.Button(gameScene.name, _styles.item))
				{
					OpenScene(gameScene.scenePath);
				}

				var colorMarkerRect = GUILayoutUtility.GetLastRect();
				colorMarkerRect.width = colorMarkerRect.height;
				colorMarkerRect.x += (_styles.item.padding.left - colorMarkerRect.width) * 0.5f;
				DrawColorMarker(colorMarkerRect, item.color);
			}
			else
			{
				_hasEmptyItems = true;
			}
		}
	}

	private void LoadStorage()
	{
		_storage = new Storage();
		if (EditorPrefs.HasKey(kPreferencesKey))
		{
			var preferencesJSON = EditorPrefs.GetString(kPreferencesKey);
			EditorJsonUtility.FromJsonOverwrite(preferencesJSON, _storage);
		}
	}

	private void SaveStorage()
	{
		var preferencesJSON = EditorJsonUtility.ToJson(_storage);
		EditorPrefs.SetString(kPreferencesKey, preferencesJSON);
	}

	private void PopulateItems()
	{
		var gameScenes = new List<GameSceneSO>();
		FindAssetsByType(gameScenes);

		foreach (var gameScene in gameScenes)
		{
			var id = gameScene.GetInstanceID();
			if (itemsMap.TryGetValue(id, out var item))
			{
				item.gameScene = gameScene;
			}
			else
			{
				item = new Item()
				{
					gameScene = gameScene,
					id = gameScene.GetInstanceID()
				};
				items.Add(item);
				itemsMap.Add(id, item);
			}
		}
	}

	private void RemoveEmptyItemsIfRequired()
	{
		if (_hasEmptyItems)
		{
			for (int i = items.Count - 1; i >= 0; --i)
			{
				var sceneItem = items[i];
				if (sceneItem == null || sceneItem.gameScene == null)
				{
					items.RemoveAt(i);
					itemsMap.Remove(sceneItem.id);
				}
			}
			items.RemoveAll((x) => x == null || x.gameScene == null);
		}
		_hasEmptyItems = false;
	}

	private void OnGameSceneSOCreated(GameSceneSO _)
	{
		RunOnNextUpdate(PopulateItems);
	}

	private void RunOnNextUpdate(Action action)
	{
		void Run()
		{
			action?.Invoke();
			EditorApplication.update -= Run;
		}
		EditorApplication.update += Run;
	}

	private void EnsureStyles()
	{
		if (_styles == null)
		{
			_styles = new Styles();

			_styles.item = "MenuItem";
			_styles.item.padding.left = kItemContentLeftPadding;
		}
	}

	private void OpenPreferences()
	{
		_preferencesWindow = PreferencesWindow.Open(this);
	}

	private static void RepaintOnMouseMove(EditorWindow window)
	{
		if (Event.current.type == EventType.MouseMove)
			window.Repaint();
	}

	private static bool DrawColorMarker(Rect rect, Color color, bool isClickable = false, bool isHoverable = false)
	{
		bool isClicked = false;
		if (isClickable)
			isClicked = GUI.Button(rect, GUIContent.none, GUIStyle.none);

		var currentEvent = Event.current;
		var isHovered = isHoverable && rect.Contains(currentEvent.mousePosition);
		var targetSize = isHovered ? kColorMarkerHoveredSize : kColorMarkerNormalSize;

		var size = rect.size;
		rect.size = new Vector2(targetSize, targetSize);
		rect.position += (size - rect.size) * 0.5f;

		Rect shadowRect = rect;
		shadowRect.position -= Vector2.one;
		shadowRect.size += Vector2.one;
		Rect lightRect = rect;
		lightRect.size += Vector2.one;

		GUIUtility.RotateAroundPivot(45.0f, rect.center);
		EditorGUI.DrawRect(shadowRect, color * kColorMarkerDarkTint);
		EditorGUI.DrawRect(lightRect, kColorMarkerLightTint);
		EditorGUI.DrawRect(rect, color);
		GUIUtility.RotateAroundPivot(-45.0f, rect.center);

		return isClicked;
	}

	private static void OpenScene(string path)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			EditorSceneManager.OpenScene(path);
		}
	}

	private static int FindAssetsByType<T>(List<T> assets) where T : UnityEngine.Object
	{
		int foundAssetsCount = 0;
		var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
		for (int i = 0, count = guids.Length; i < count; ++i)
		{
			var path = AssetDatabase.GUIDToAssetPath(guids[i]);
			T asset = AssetDatabase.LoadAssetAtPath<T>(path);
			if (asset != null)
			{
				assets.Add(asset);
				foundAssetsCount += 1;
			}
		}
		return foundAssetsCount;
	}

	void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
	{
		menu.AddItem(kOpenPreferencesItemContent, false, OpenPreferences);
	}
}

public static class KeyValuePairExtension
{
	public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
	{
		key = tuple.Key;
		value = tuple.Value;
	}
}
