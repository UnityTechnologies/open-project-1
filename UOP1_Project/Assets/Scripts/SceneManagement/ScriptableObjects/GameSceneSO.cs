using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
/// </summary>

public abstract partial class GameSceneSO : ScriptableObject
{
	[Header("Information")]
#if UNITY_EDITOR // See GameSceneSOEditor.cs
	public UnityEditor.SceneAsset sceneAsset; //Just used in editor in the SceneLoading tool for the purpose of quickly opening scenes
#endif
	[HideInInspector] public string scenePath;
	public AssetReference sceneReference; //Used at runtime to load the scene from the right AssetBundle

	[TextArea] public string shortDescription;

	[Header("Sounds")]
	public AudioClip music;
}
