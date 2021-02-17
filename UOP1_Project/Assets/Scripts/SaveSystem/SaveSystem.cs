using UnityEngine;

public class SaveSystem : ScriptableObject
{
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private Inventory _playerInventory;

	public string saveFilename = "save.chop";
	public Save saveData = new Save();

	void OnEnable()
	{
		_loadLocation.OnLoadingRequested += CacheLoadLocations;
	}

	void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= CacheLoadLocations;
	}

	private void CacheLoadLocations(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		LocationSO locationSo = locationsToLoad[0] as LocationSO;
		if (locationSo)
		{
			saveData._locationId = locationSo.SerializableGuid;
		}

		SaveGame();
	}

	public void LoadGame()
	{
		if (FileManager.LoadFromFile(saveFilename, out var json))
		{
			saveData.LoadFromJson(json);
		}
	}


	public void SaveGame()
	{
		saveData._itemStacks.Clear();
		foreach (var itemStack in _playerInventory.Items)
		{
			saveData._itemStacks.Add(new SerializedItemStack(itemStack.Item.SerializableGuid, itemStack.Amount));
		}

		if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
		{
			Debug.Log("Save successful");
		}
	}
}
