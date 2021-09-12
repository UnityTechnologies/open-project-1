using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SceneSelectorInternal;
using UnityEditor.SceneManagement;
using SceneType = GameSceneSO.GameSceneType;

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

	[MenuItem("ChopChop/Scene Selector")]
	private static void Open()
	{
		GetWindow<SceneSelector>();
	}

	private void OnEnable()
	{
		wantsMouseMove = true;
		LoadStorage();
		PopulateItems();
	}

	private void OnDisable()
	{
		if (_preferencesWindow != null)
			_preferencesWindow.Close();
		SaveStorage();
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

		if (GUILayout.Button("Reset list"))
		{
			//Force deletion of the storage
			_storage = new Storage();
			EditorPrefs.SetString(kPreferencesKey, "");

			OnEnable(); //search the project and populate the scene list again
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
			var gameSceneSO = item.gameSceneSO;
			if (gameSceneSO != null)
			{
				if (GUILayout.Button(gameSceneSO.name, _styles.item))
				{
					Helper.OpenSceneSafe(gameSceneSO);
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
		var gameSceneSOs = new List<GameSceneSO>();
		Helper.FindAssetsByType(gameSceneSOs);

		foreach (var gameSceneSO in gameSceneSOs)
		{
			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(gameSceneSO, out var guid, out long _))
			{
				if (itemsMap.TryGetValue(guid, out var item))
				{
					item.gameSceneSO = gameSceneSO;
				}
				else
				{
					item = new Item()
					{
						gameSceneSO = gameSceneSO,
						guid = guid,
						color = Helper.GetDefaultColor(gameSceneSO)
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
				if (sceneItem == null || sceneItem.gameSceneSO == null)
				{
					items.RemoveAt(i);
					itemsMap.Remove(sceneItem.guid);
				}
			}
		}
		_hasEmptyItems = false;
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
