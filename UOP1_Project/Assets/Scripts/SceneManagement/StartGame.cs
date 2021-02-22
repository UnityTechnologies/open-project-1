using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	public SaveSystem saveSystem;

	public void OnPlayButtonPress()
	{
		onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}

	public void OnLoadButtonPress()
	{
		StartCoroutine(LoadSaveGame());
	}

	public IEnumerator LoadSaveGame()
	{
		saveSystem.LoadSaveDataFromDisk();
		yield return StartCoroutine(saveSystem.LoadSavedInventory());

		var locationGuid = saveSystem.saveData._locationId;
		var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
		yield return asyncOperationHandle;
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			var locationSo = asyncOperationHandle.Result;
			onPlayButtonPress.RaiseEvent(new[] { (GameSceneSO)locationSo }, showLoadScreen);
		}
	}
}
