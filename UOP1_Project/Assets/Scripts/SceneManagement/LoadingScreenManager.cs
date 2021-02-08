using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
	[Header("Loading screen Event")]
	//The loading screen event we are listening to
	[SerializeField] private BoolEventChannelSO _ToggleLoadingScreen = default;

	[Header("Loading screen ")]
	public GameObject loadingInterface;

	private void OnEnable()
	{
		if (_ToggleLoadingScreen != null)
		{
			_ToggleLoadingScreen.RegisterEvent(ToggleLoadingScreen);
		}
	}

	private void OnDisable()
	{
		if (_ToggleLoadingScreen != null)
		{
			_ToggleLoadingScreen.UnregisterEvent(ToggleLoadingScreen);
		}
	}

	private void ToggleLoadingScreen(bool state)
	{
		loadingInterface.SetActive(state);
	}

}
