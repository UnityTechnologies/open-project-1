using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIDebug : MonoBehaviour
{
    [SerializeField] private DebugConfigSO debugConfig;
    [SerializeField] private Button[] locationButtons;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject locationsParent;

    public UnityAction buttonClickAction;

    public UnityAction Closed; 


    private void Start()
    {
        locationButtons = new Button[debugConfig.locations.Length]; 

        for (int i = 0; i < debugConfig.locations.Length; i++)
        {
            GameObject locationButton = Instantiate(buttonPrefab);
            locationButton.name = "Location Item " + i;
            locationButton.transform.SetParent(locationsParent.gameObject.transform, false);

            TextMeshProUGUI text = locationButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.SetText(debugConfig.locations[i].name);


            //Debug.Log(i);
            locationButtons[i] = locationButton.GetComponent<Button>();
        }
    }

    public void CloseDebugMenu()
	{
		Closed.Invoke();
	}
}
