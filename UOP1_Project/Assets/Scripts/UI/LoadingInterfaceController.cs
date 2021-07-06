using UnityEngine;

public class LoadingInterfaceController : MonoBehaviour
{
	[Header("Loading screen Event")]
	//The loading screen event we are listening to
	[SerializeField] private BoolEventChannelSO _ToggleLoadingScreen = default;

	[Header("Loading screen ")]
	public GameObject loadingInterface;

	private void OnEnable()
	{
		_ToggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
	}

	private void OnDisable()
	{
		_ToggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
	}

	private void ToggleLoadingScreen(bool state)
	{
		loadingInterface.SetActive(state);
	}

}
