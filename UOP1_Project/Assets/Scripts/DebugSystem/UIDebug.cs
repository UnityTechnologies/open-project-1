using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles UI logic for the debug menu.
/// </summary>
public class UIDebug : MonoBehaviour
{
    [Header("Debug Stuff")]
    [SerializeField] private DebugConfigSO debugConfig;
    [SerializeField] private Button[] locationButtons;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject locationsParent;

    [Header("Broadcasting On")]
    [SerializeField] private LoadEventChannelSO locationLoadEventChannel;

    public UnityAction Closed;

    private void OnEnable()
    {
        RemoveOldButtons();
        DrawButtons();
    }

    public void RemoveOldButtons()
    {
        for (int i = 0; i < locationButtons.Length; i++)
        {
            //Debug.Log("Removing " + locationButtons[i].gameObject, gameObject);
            Destroy(locationButtons[i].gameObject);
        }
    }

    private void DrawButtons()
    {
        locationButtons = new Button[debugConfig.locations.Length]; 

        for (int i = 0; i < debugConfig.locations.Length; i++)
        {
            GameObject locationButton = Instantiate(buttonPrefab);
            locationButton.name = "Location Item " + i;
            locationButton.transform.SetParent(locationsParent.gameObject.transform, false);

            Button button = locationButton.GetComponent<Button>();
            // Debug.Log(button);
            // Debug.Log(i);

            // button.onClick.AddListener(delegate{
            //     OnLocationButtonClick(i);
            // });
            //button.onClick.AddListener(() => OnLocationButtonClick(i));

            TextMeshProUGUI text = locationButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.SetText(debugConfig.locations[i].name);

            if (SceneManager.GetSceneByName(debugConfig.locations[i].sceneReference.editorAsset.name).isLoaded)
            {
                button.interactable = false;
            }


            //Debug.Log(i);
            locationButtons[i] = button;
        }

        for (int i = 0; i < debugConfig.locations.Length; i++)
        {
            // This 'index' variable is created here to avoid a situation where if we pass variable 'i' to the delegate call, 
            // it would increment with each loop and finally be the length of the loop, which is here, 10.
            // Then 10 will be passed to 'OnLocationButtonClick', for each and every OnClick Listener, which is OutOfRange for an array of length 10.
            // and Hence the error rises. To see this youself, just comment the below line and pass i instead of index in the OnLocationButtonClick call.
            int index = i;
            // To understand more about this, please refer here (https://programmerought.com/article/82934039881/), where I also got the solution.

            // Adding extra parameters to Buttons OnClick event using delegate from here: http://answers.unity.com/answers/1378134/view.html
            locationButtons[i].onClick.AddListener(delegate{
                OnLocationButtonClick(index);
            });
        }
    }

    public void OnLocationButtonClick(int locationIndex)
    {
        locationLoadEventChannel.RaiseEvent(debugConfig.locations[locationIndex], false, true);
    }

    public void CloseDebugMenu()
	{
		Closed.Invoke();
	}
}
