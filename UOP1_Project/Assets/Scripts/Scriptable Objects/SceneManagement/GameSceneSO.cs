using UnityEngine;

/// <summary>
/// This class is a base class which contains what is commun for all game scenes (Locations or Menus)
/// </summary>

public class GameSceneSO : ScriptableObject
{
	[Header("Information")]
	public string sceneName;
	public string shortDescription;

	[Header("Sounds")]
	public AudioClip music;
	[Range(0.0f, 1.0f)]
	public float musicVolume;

}
