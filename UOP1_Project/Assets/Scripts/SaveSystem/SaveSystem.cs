using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
			saveData._locationId = locationSo.Guid;
		}

		SaveDataToDisk();
	}

	public void LoadSaveDataFromDisk()
	{
		if (FileManager.LoadFromFile(saveFilename, out var json))
		{
			saveData.LoadFromJson(json);
		}
	}

	public IEnumerator LoadSavedInventory()
	{
		_playerInventory.Items.Clear();
		foreach (var serializedItemStack in saveData._itemStacks)
		{
			var loadItemOperationHandle = Addressables.LoadAssetAsync<Item>(serializedItemStack.itemGuid);
			yield return loadItemOperationHandle;
			if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				var itemSo = loadItemOperationHandle.Result;
				_playerInventory.Add(itemSo, serializedItemStack.amount);
			}
		}
	}

	public void SaveDataToDisk()
	{
		saveData._itemStacks.Clear();
		foreach (var itemStack in _playerInventory.Items)
		{
			saveData._itemStacks.Add(new SerializedItemStack(itemStack.Item.Guid, itemStack.Amount));
		}

		if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
		{
			Debug.Log("Save successful");
		}
	}
}
