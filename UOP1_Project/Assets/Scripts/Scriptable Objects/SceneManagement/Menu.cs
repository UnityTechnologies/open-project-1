using UnityEngine;

/// <summary>
/// This class contains Settings specific to Menus only
/// </summary>

public enum Type
{
	Main_Menu,
	Pause_Menu
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class Menu : GameScene
{
	[Header("Menu specific")]
	public Type type;
}
