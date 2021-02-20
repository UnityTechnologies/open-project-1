using System.Linq;
using System.Reflection;

public static class Tag
{
	//Pre-defined Tags
	public const string
	Untagged = "Untagged",
	Respawn = "Respawn",
	Finish = "Finish",
	EditorOnly = "EditorOnly",
	MainCamera = "MainCamera",
	Player = "Player",
	GameController = "GameController";
	//Custom Tags
	public const string
	Pickable = "Pickable",
	CookingPot = "CookingPot",
	NPC = "NPC",
	SpawnLocation = "SpawnLocation",
	Critter = "Critter";
		
	/// <summary>
	/// Returns all tags that has been defined in Tag class
	/// </summary>
	public static string[] GetTags()
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
	/// Checks if the tag defined in the Tag class
	/// </summary>
	public static bool TagExist(string search)
	{
		string[] AllTags = GetTags();
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
