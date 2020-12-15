using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelector : EditorWindow
{
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
	private ReorderableList _itemsReorderableList;
	private List<Item> _visibleItems = new List<Item>();
	private List<Item> _hiddenItems = new List<Item>();
	private Vector2 _windowScrollPosition;
	private bool _hasEmptyItems;
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
		CreateReorderableList();
		PopulateItems();
		GameSceneSO.onEnabled += OnGameSceneSOCreated;
	}

	private void OnDisable()
	{
		SaveStorage();
		GameSceneSO.onEnabled -= OnGameSceneSOCreated;
	}

	private void OnGUI()
	{
		EnsureStyles();
		RemoveEmptyItemsIfRequired();
		DrawWindow();
	}

	private void CreateReorderableList()
	{
		_itemsReorderableList = new ReorderableList(items, typeof(Item), true, true, false, false);
		_itemsReorderableList.drawElementCallback = DrawItem;
		_itemsReorderableList.drawHeaderCallback = DrawListHeader;
		_itemsReorderableList.elementHeightCallback = GetGameSceneItemHeight;
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

	private void DrawListHeader(Rect rect)
	{ }

	private float GetGameSceneItemHeight(int index)
	{
		const float kZeroHeight = 0.0f;
		const float kNormalHeight = 20.0f;
		var item = items[index];
		var isVisible = item.isVisible || _showHiddenItems;
		return isVisible
			? kNormalHeight
			: kZeroHeight;
	}

	private void DrawItemBackground(Rect rect, int index, bool isActive, bool isFocused)
	{ }

	private void DrawItem(Rect rect, int index, bool isActive, bool isFocused)
	{
		var item = items[index];
		var gameScene = item.gameScene;
		if (gameScene != null)
		{
			if (item.isVisible || _showHiddenItems)
			{
				var visWidth = rect.height;

				var buttonRect = rect;
				buttonRect.width -= visWidth;

				var visRect = new Rect(rect);
				visRect.x = buttonRect.x + buttonRect.width;
				visRect.width = visWidth;
				visRect.height = visWidth;

				if (GUI.Button(buttonRect, gameScene.name))
				{
					OpenScene(gameScene.scenePath);
				}

				var visibilityIcon = item.isVisible
				? _textures.visibilityOn
				: _textures.visibilityOff;
				if (GUI.Button(visRect, visibilityIcon, _styles.button))
				{
					item.isVisible = !item.isVisible;
				}
			}
		}
		else
		{
			_hasEmptyItems = true;
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

	private void DrawWindow()
	{
		using (var scrollScope = new EditorGUILayout.ScrollViewScope(_windowScrollPosition))
		{
			DrawToolbar();
			_itemsReorderableList.DoLayoutList();
			_windowScrollPosition = scrollScope.scrollPosition;
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

	private void RunOnNextUpdate(Action action)
	{
		void Run()
		{
			action?.Invoke();
			EditorApplication.update -= Run;
		}
		EditorApplication.update += Run;
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
