using UnityEngine;

/// <summary>
/// This class contains Settings specific to Menus only
/// </summary>

public enum Menu
{
	Main_Menu,
	Pause_Menu
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class MenuSO : GameSceneSO
{
	[Header("Menu specific")]
	public Menu menuType;
}
