using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Components;
using UnityEngine.UI;
public class InventoryTypeTabFiller : MonoBehaviour
{

	[SerializeField] private Image _tabImage = default;

	[SerializeField] private Button _actionButton = default;

	[SerializeField] private Color _selectedIconColor = default;
	[SerializeField] private Color _deselectedIconColor = default;

	[SerializeField] private TabEventChannelSO _changeTabEvent = default;



	public void fillTab(InventoryTabType tabType, bool isSelected)
	{

		_tabImage.sprite = tabType.TabIcon;
		_actionButton.interactable = !isSelected;

		if (isSelected)
		{
			_tabImage.color = _selectedIconColor;
		}
		else
		{
			_tabImage.color = _deselectedIconColor;
		}

		_actionButton.onClick.RemoveAllListeners();
		_actionButton.onClick.AddListener(() => _changeTabEvent.RaiseEvent(tabType));
	}

}
