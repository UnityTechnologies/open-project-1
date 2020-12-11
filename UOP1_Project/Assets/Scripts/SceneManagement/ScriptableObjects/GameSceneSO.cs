using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
/// </summary>

public abstract class GameSceneSO : ScriptableObject
#if UNITY_EDITOR
	, ISerializationCallbackReceiver
#endif
{
	[Header("Information")]
#if UNITY_EDITOR
	public SceneAsset sceneAsset;
#endif
	[HideInInspector]
	public string scenePath;
	public string shortDescription;

	[Header("Sounds")]
	public AudioClip music;

#if UNITY_EDITOR
	private SceneAsset prevSceneAsset;

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		PopulateScenePath();
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{ }

	private void PopulateScenePath()
	{
		if (sceneAsset != null)
		{
			// To prevent constant invocation of AssetDatabase API
			// when this SO is opened in the Inspector.
			if (prevSceneAsset != sceneAsset)
			{
				prevSceneAsset = sceneAsset;
				scenePath = AssetDatabase.GetAssetPath(sceneAsset);
			}
		}
		else
		{
			scenePath = string.Empty;
		}
	}
#endif
}
