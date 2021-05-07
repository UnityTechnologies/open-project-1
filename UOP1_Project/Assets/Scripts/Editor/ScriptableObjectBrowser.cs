using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This editor window allows you to quickly locate and edit any ScriptableObject assets in the project folder.
/// </summary>
class ScriptableObjectBrowser : EditorWindow
{
	private SortedDictionary<string, System.Type> _types = new SortedDictionary<string, System.Type>();
	private Dictionary<string, ScriptableObject> _assets = new Dictionary<string, ScriptableObject>();
	private Vector2 _typeScrollViewPosition;
	private Vector2 _assetScrollViewPosition;
	private int _typeIndex;
	private int _lastAssetIndex;
	private bool _showingTypes = true;
	private static GUIStyle _buttonStyle;

	public static GUIStyle ButtonStyle
	{
		get
		{
			if (_buttonStyle == null)
			{
				_buttonStyle = EditorStyles.toolbarButton;
				_buttonStyle.alignment = TextAnchor.MiddleLeft;
			}

			return _buttonStyle;
		}
	}

	private void OnEnable()
	{
		LoadData();
	}

	private void OnFocus()
	{
		LoadData();
	}

	private void LoadData()
	{
		if (_showingTypes)
		{
			GetTypes();
		}
		else
		{
			GetAssets();
		}
	}

	[MenuItem("ChopChop/Scriptable Object Browser")]
	private static void ShowWindow()
	{
		GetWindow<ScriptableObjectBrowser>("Scriptable Objects");
	}

	private void OnGUI()
	{
		if (_showingTypes)
		{
			GUILayout.Label("Scriptable Object Types", EditorStyles.largeLabel);

			if (GUILayout.Button("Refresh List"))
			{
				GetTypes();
			}

			DrawTypeButtons();
		}
		else
		{
			GUILayout.Label(GetNiceName(_types.ElementAt(_typeIndex).Key), EditorStyles.largeLabel);

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Refresh List"))
			{
				GetAssets();
			}

			if (GUILayout.Button("Back to Types"))
			{
				GetTypes();
				_showingTypes = true;
			}

			GUILayout.EndHorizontal();

			DrawAssetButtons();
		}
	}

	/// <summary>
	/// Draws a scroll view list of Buttons for each ScriptableObject type.
	/// </summary>
	private void DrawTypeButtons()
	{
		_typeScrollViewPosition = GUILayout.BeginScrollView(_typeScrollViewPosition);

		for (int i = 0; i < _types.Count; i++)
		{
			if (GUILayout.Button(GetNiceName(_types.ElementAt(i).Key), EditorStyles.foldout))
			{
				_typeIndex = i;
				GetAssets();
				_showingTypes = false;
			}
		}

		GUILayout.EndScrollView();
	}

	/// <summary>
	/// Draws a scroll view list of Buttons for each ScriptableObject asset file of selected type. 
	/// </summary>
	private void DrawAssetButtons()
	{
		_assetScrollViewPosition = GUILayout.BeginScrollView(_assetScrollViewPosition);

		for (int i = 0; i < _assets.Count; i++)
		{
			if (GUILayout.Button(GetNiceName(_assets.ElementAt(i).Value.name), ButtonStyle))
			{
				Selection.activeObject = _assets.ElementAt(i).Value;

				if (_lastAssetIndex == i)
				{
					EditorGUIUtility.PingObject(_assets.ElementAt(i).Value);
				}

				_lastAssetIndex = i;
			}
		}

		GUILayout.EndScrollView();
	}

	/// <summary>
	/// Gets all ScriptableObject types in project.
	/// </summary>
	private void GetTypes()
	{
		string[] GUIDs = AssetDatabase.FindAssets("t:ScriptableObject",
			new string[] { "Assets/ScriptableObjects" });
		ScriptableObject[] SOs = new ScriptableObject[GUIDs.Length];

		for (int i = 0; i < GUIDs.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
			SOs[i] = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
		}

		_types.Clear();

		for (int i = 0; i < SOs.Length; i++)
		{
			string typeKey = SOs[i].GetType().Name;

			if (!_types.ContainsKey(typeKey))
			{
				_types.Add(typeKey, SOs[i].GetType());
			}
		}
	}

	/// <summary>
	/// Gets all ScriptableObject asset files of selected type.
	/// </summary>
	private void GetAssets()
	{
		string[] GUIDs = AssetDatabase.FindAssets("t:" + _types.ElementAt(_typeIndex).Value.FullName);

		_assets.Clear();

		for (int i = 0; i < GUIDs.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
			var SO = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject)) as ScriptableObject;
			_assets.Add(path, SO);
		}
	}

	/// <summary>
	/// Formats string of text to look prettier and more readable.
	/// </summary>
	private string GetNiceName(string text)
	{
		string niceText = text;
		niceText = ObjectNames.NicifyVariableName(niceText);
		niceText = niceText.Replace(" SO", "");
		niceText = niceText.Replace("-", " ");
		niceText = niceText.Replace("_", " ");
		return niceText;
	}
}
