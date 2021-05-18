using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{	
	[SerializeField]
	private GameSceneSO _locationsToLoad;
	[SerializeField]
	private  bool _showLoadScreen = default;
	[SerializeField]
	private SaveSystem _saveSystem = default ;
	private bool _hasSaveData;
	[Header("Broadcasting on ")]
	[SerializeField]
	private LoadEventChannelSO _startGameEvent = default;
	[Header("Listening to")]
	[SerializeField]
	private VoidEventChannelSO _startNewGameEvent = default;
	[SerializeField]
	private VoidEventChannelSO _continueGameEvent = default;


	private void Start()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_startNewGameEvent.OnEventRaised += StartNewGame;
		_continueGameEvent.OnEventRaised += ContinuePreviousGame; 
	}
	void StartNewGame()
	{
		_hasSaveData = false;
		_saveSystem.WriteEmptySaveFile();
		//Start new game
		_startGameEvent.RaiseEvent(_locationsToLoad, _showLoadScreen);
		
		
	}
	void ContinuePreviousGame()
	{
		StartCoroutine(LoadSaveGame());
	}
	

	 void OnResetSaveDataPress()
	{
		_hasSaveData = false;

	}

	 IEnumerator LoadSaveGame()
	{
		yield return StartCoroutine(_saveSystem.LoadSavedInventory());

		var locationGuid = _saveSystem.saveData._locationId;
		var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
		yield return asyncOperationHandle;
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			LocationSO locationSO = asyncOperationHandle.Result;
			_startGameEvent.RaiseEvent(locationSO, _showLoadScreen);
		}
	}
}
