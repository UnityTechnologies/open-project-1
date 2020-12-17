using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Components;
using UnityEngine.UI;
public class InventoryTypeTabFiller : MonoBehaviour
{

	[SerializeField]
	private LocalizeStringEvent tabName;

	[SerializeField]
	private Button actionButton;

	public void fillTab(InventoryTabType tabType, bool isSelected, TabEventChannelSo changeTabEvent)
	{

		tabName.StringReference = tabType.TabName;
		actionButton.interactable = !isSelected;
		actionButton.onClick.RemoveAllListeners();
		actionButton.onClick.AddListener(() => changeTabEvent.RaiseEvent(tabType));

	}


}
