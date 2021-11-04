using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private GameObject _menuPrefab;
	private GameObject _menuInstance;

	private void OpenMenu()
	{
		if (_menuInstance == null)
			_menuInstance = Instantiate(_menuPrefab);

		_menuInstance.SetActive(true);
		_inputReader.EnableMenuInput();
	}

	private void UnpauseMenu()
	{
		_menuInstance.SetActive(false);
		_inputReader.EnableGameplayInput();
	}
}
