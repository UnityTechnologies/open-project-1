using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Components;
using UnityEngine.UI;
public class InventoryTypeTabFiller : MonoBehaviour
{

	[SerializeField] private LocalizeStringEvent _tabName = default;

	[SerializeField] private Button _actionButton = default;

	public void fillTab(InventoryTabType tabType, bool isSelected, TabEventChannelSO changeTabEvent)
	{

		_tabName.StringReference = tabType.TabName;
		_actionButton.interactable = !isSelected;
		_actionButton.onClick.RemoveAllListeners();
		_actionButton.onClick.AddListener(() => changeTabEvent.RaiseEvent(tabType));

	}


}
