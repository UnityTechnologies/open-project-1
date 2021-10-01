using System;
using UnityEngine;

public class EditorTeleporter : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
	[SerializeField] private GameObject _cheatMenu;
	[SerializeField] private PathStorageSO _path;

	[Header("Broadcast on")]
	[SerializeField] private LoadEventChannelSO _loadLocationRequest;

	private LocationSO _lastLocationTeleportedTo = default;

	private void OnEnable() => _inputReader.CheatMenuEvent += ToggleCheatMenu;

	private void OnDisable() => _inputReader.CheatMenuEvent -= ToggleCheatMenu;

	private void Start()
	{
		_cheatMenu.SetActive(false);
	}

	private void ToggleCheatMenu()
	{
		_cheatMenu.SetActive(!_cheatMenu.activeInHierarchy);
	}

	public void Teleport(LocationSO where, PathSO whichEntrance)
	{
		//Avoid reloading the same Location, which would result in an error
		if(where == _lastLocationTeleportedTo)
			return;

		_path.lastPathTaken = whichEntrance;
		_lastLocationTeleportedTo = where;
		_loadLocationRequest.RaiseEvent(where);
	}
}
