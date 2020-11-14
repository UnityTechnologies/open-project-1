using System;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
	[SerializeField] private GameObject _menuPrefab;
	private GameObject _menuInstance;
	[SerializeField] private InputReader _inputReader;
	private void OnEnable()
	{
		_inputReader.pauseEvent += OpenMenu;
		_inputReader.Menu.CloseMenuEvent += CloseMenu;
	}

	private void OnDisable()
	{
		_inputReader.pauseEvent -= OpenMenu;
		_inputReader.Menu.CloseMenuEvent -= CloseMenu;
	}

	private void OpenMenu()
	{
		if (_menuInstance == null) _menuInstance = Instantiate(_menuPrefab);
		_menuInstance.SetActive(true);
		_inputReader.EnableMenuInput();
	}

	private void CloseMenu()
	{
		_menuInstance.SetActive(false);
		_inputReader.EnableGameplayInput();
	}
}
