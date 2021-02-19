using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Tag
{
	//Pre-defined Tags
	public const string Untagged = "Untagged";
	public const string Respawn = "Respawn";
	public const string Finish = "Finish";
	public const string EditorOnly = "EditorOnly";
	public const string MainCamera = "MainCamera";
	public const string Player = "Player";
	public const string GameController = "GameController";

	//Custom Tags
	public const string Pickable = "Pickable";
	public const string CookingPot = "CookingPot";
	public const string NPC = "NPC";
	public const string SpawnLocation = "SpawnLocation";
	public const string Critter = "Critter";
		
	/// <summary>
	/// Returns all tags that has been defined in Tags class
	/// </summary>
	public static string[] GetAllTags()
	{
		FieldInfo[] fields = typeof(Tag).GetFields(BindingFlags.Public | BindingFlags.Static |
			   BindingFlags.FlattenHierarchy).Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToArray();

		string[] strArray = new string[fields.Length];
		for (int i = 0; i < fields.Length; i++)
		{
			strArray[i] = fields[i].Name;
		}
		return strArray;
	}

	/// <summary>
	/// Checks if the tag defined in the Tags class
	/// </summary>
	public static bool TagExist(string search)
	{
		string[] AllTags = GetAllTags();
		bool result = false;
		for (int i = 0; i < AllTags.Length; i++)
		{
			if (AllTags[i] == search)
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
