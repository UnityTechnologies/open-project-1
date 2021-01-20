using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SceneSelectorInternal;

public partial class SceneSelector : EditorWindow, IHasCustomMenu
{
	private const string kPreferencesKey = "uop1.SceneSelector.Preferences";
	private const int kItemContentLeftPadding = 32;
	private static readonly GUIContent kOpenPreferencesItemContent = new GUIContent("Open Preferences");

	private Styles _styles;
	private Storage _storage;
	private PreferencesWindow _preferencesWindow;
	private Vector2 _windowScrollPosition;
	private bool _hasEmptyItems;

	private List<Item> items => _storage.items;
	private Dictionary<string, Item> itemsMap => _storage.itemsMap;

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
		Helper.RepaintOnMouseMove(this);
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
					Helper.OpenSceneSafe(gameScene.scenePath);
				}

				var colorMarkerRect = GUILayoutUtility.GetLastRect();
				colorMarkerRect.width = colorMarkerRect.height;
				colorMarkerRect.x += (_styles.item.padding.left - colorMarkerRect.width) * 0.5f;
				Helper.DrawColorMarker(colorMarkerRect, item.color);
			}
			else
			{
				// In case GameSceneSO was removed (see RemoveEmptyItemsIfRequired)
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
		Helper.FindAssetsByType(gameScenes);

		foreach (var gameScene in gameScenes)
		{
			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(gameScene, out var guid, out long _))
			{
				if (itemsMap.TryGetValue(guid, out var item))
				{
					item.gameScene = gameScene;
				}
				else
				{
					item = new Item()
					{
						gameScene = gameScene,
						guid = guid,
						color = Helper.GetDefaultColor(gameScene)
					};
					items.Add(item);
					itemsMap.Add(guid, item);
				}
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
					itemsMap.Remove(sceneItem.guid);
				}
			}
		}
		_hasEmptyItems = false;
	}

	private void OnGameSceneSOCreated(GameSceneSO _)
	{
		Helper.RunOnNextUpdate(PopulateItems);
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

	void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
	{
		menu.AddItem(kOpenPreferencesItemContent, false, OpenPreferences);
	}
}
