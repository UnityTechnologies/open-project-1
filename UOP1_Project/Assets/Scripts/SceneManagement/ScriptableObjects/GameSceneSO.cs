using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
/// </summary>

public class GameSceneSO : DescriptionBaseSO
{
	public AssetReference sceneReference; //Used at runtime to load the scene from the right AssetBundle
}
