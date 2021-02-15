using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	public delegate void AddToRegistryCallback(HashSet<ISaveable> registry);

	public static AddToRegistryCallback AddToRegistry;

	HashSet<ISaveable> _saveRegistry = default;

	const string SAVE_FILENAME = "save.chop";

	private void Start()
	{
		Debug.Log("SaveSystem start getting called");
		_saveRegistry = new HashSet<ISaveable>();
		AddToRegistry?.Invoke(_saveRegistry);
		LoadGame();
	}

	private void LoadGame()
	{
		if (FileManager.LoadFromFile(SAVE_FILENAME, out var json))
		{
			Save saveData = new Save();
			saveData.LoadFromJson(json);

			foreach (ISaveable saveable in _saveRegistry)
			{
				saveable.Deserialize(saveData);
			}
		}
	}

	private void SaveGame()
	{
		// A class with name "Save" must exist in the project. It will contain the appropriate save file structure.
		Save saveData = new Save();
		foreach (ISaveable saveable in _saveRegistry)
		{
			saveable.Serialize(saveData);
		}

		if (FileManager.WriteToFile(SAVE_FILENAME, saveData.ToJson()))
		{
			Debug.Log("Save successful");
		}
	}

	private void OnDisable()
	{
		SaveGame();
	}
}
