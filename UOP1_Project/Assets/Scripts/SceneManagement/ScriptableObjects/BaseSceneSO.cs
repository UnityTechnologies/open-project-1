using UnityEngine;

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
/// </summary>

public class BaseSceneSO : ScriptableObject
{
	[Header("Information")]
	public string SceneName;
	public string SceneDescription;

	[Header("Sounds")]
	public AudioClip Music;

}
