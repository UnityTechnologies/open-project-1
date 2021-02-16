using UnityEngine;

public class SaveSystem : ScriptableObject
{
	public LocationDatabase locationDatabase;
	[SerializeField] private LoadEventChannelSO _loadLocation = default;

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
			saveData._locationId = locationSo.DescId.uuid;
		}
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
		if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
		{
			Debug.Log("Save successful");
		}
	}
}
