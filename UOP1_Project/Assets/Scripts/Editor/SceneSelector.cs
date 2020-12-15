using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelector : EditorWindow
{
	public class PreferencesWindow : EditorWindow
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

		private static readonly Color kItemBorderColor = new Color(1.0f, 1.0f, 1.0f, 0.16f);

		private SceneSelector _owner;
		private ReorderableList _itemsReorderableList;
		private Styles _styles;
		private Vector2 _windowScrollPosition;
		private bool _hasEmptyItems;

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
			GameSceneSO.onEnabled += OnGameSceneSOCreated;
		}

		private void OnDisable()
		{
			_owner.SaveStorage();
			GameSceneSO.onEnabled -= OnGameSceneSOCreated;
		}

		private void Init(SceneSelector owner)
		{
			_owner = owner;
			CreateReorderableList();
			PopulateItems();
		}

		private void OnGUI()
		{
			EnsureStyles();
			RemoveEmptyItemsIfRequired();
			HandleEvents();
			DrawWindow();
		}

		private void RepaintAll()
		{
			Repaint();
			_owner.Repaint();
		}

		private void CreateReorderableList()
		{
			_itemsReorderableList = new ReorderableList(items, typeof(Item), true, true, false, false);
			_itemsReorderableList.drawElementCallback = DrawItem;
			_itemsReorderableList.drawElementBackgroundCallback = DrawItemBackground;
			_itemsReorderableList.headerHeight = kHeaderHeight;
			_itemsReorderableList.elementHeight = kItemHeight;
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

		private void HandleEvents()
		{
			if (Event.current.type == EventType.MouseMove)
				RepaintAll();
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
				var itemLabelRect = rect;
				itemLabelRect.width -= kVisibilityButtonSize;

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
				}
			}
			else
			{
				_hasEmptyItems = true;
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

		[NonSerialized]
		public GameSceneSO gameScene;
	}

	private class Styles
	{
		public GUIStyle button;
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

	private Styles _styles;
	private Textures _textures;
	private Storage _storage;
	private Vector2 _windowScrollPosition;
	private bool _showHiddenItems;

	private List<Item> items => _storage.items;
	private Dictionary<int, Item> itemsMap => _storage.itemsMap;

	[MenuItem("Window/Scene Selector", false, 100)]
	private static void Open()
	{
		GetWindow<SceneSelector>();
	}

	private void OnEnable()
	{
		LoadStorage();
		LoadTextures();
		PreferencesWindow.Open(this);
	}

	private void OnDisable()
	{
		SaveStorage();
	}

	private void OnGUI()
	{
		EnsureStyles();
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
			if (GUILayout.Button(gameScene.name))
			{
				OpenScene(gameScene.scenePath);
			}
		}
	}

	private void DrawToolbar()
	{
		using (var horizontal = new EditorGUILayout.HorizontalScope())
		{
			GUILayout.FlexibleSpace();

			var visibilityIcon = _showHiddenItems
				? _textures.visibilityOn
				: _textures.visibilityOff;
			if (GUILayout.Button(visibilityIcon, _styles.button))
			{
				_showHiddenItems = !_showHiddenItems;
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

	private void EnsureStyles()
	{
		if (_styles == null)
		{
			_styles = new Styles();

			_styles.button = new GUIStyle(GUI.skin.button);
			_styles.button.padding = new RectOffset(0, 0, 0, 0);
		}
	}

	private void LoadTextures()
	{
		if (_textures == null)
		{
			_textures = new Textures();

			_textures.visibilityOn = EditorGUIUtility.FindTexture("d_scenevis_visible");
			_textures.visibilityOff = EditorGUIUtility.FindTexture("d_scenevis_hidden");
		}
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
}

public static class KeyValuePairExtension
{
	public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
	{
		key = tuple.Key;
		value = tuple.Value;
	}
}
