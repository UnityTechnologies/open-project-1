using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
public class UISetButtonNavigation : MonoBehaviour
{
	[FormerlySerializedAs("genericButtons")]
	UIGenericButton[] _genericButtons = default;
	[FormerlySerializedAs("listSettingItems")]
	UISettingItemFiller[] _listSettingItems = default;
	private void OnEnable()
	{
		if (_listSettingItems == null)
		{
			_listSettingItems = GetComponentsInChildren<UISettingItemFiller>();
		}
		if (_listSettingItems.Length > 0)
			if (_listSettingItems[0].GetComponent<MultiInputButton>() != null)
				//select first item
				_listSettingItems[0].GetComponent<MultiInputButton>().Select();
	}
	private void Start()
	{
		_listSettingItems = GetComponentsInChildren<UISettingItemFiller>();
		_genericButtons = GetComponentsInChildren<UIGenericButton>();
		MultiInputButton buttonToSelectOnDown = default;
		MultiInputButton buttonToSelectOnUp = default;
		for (int i = 0; i < _listSettingItems.Length; i++)
		{
			Navigation newNavigation = new Navigation();

			newNavigation.mode = Navigation.Mode.Explicit;

			newNavigation.selectOnLeft = _listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation.selectOnLeft;
			newNavigation.selectOnRight = _listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation.selectOnRight;

			if (i + 1 < _listSettingItems.Length)
			{
				buttonToSelectOnDown = _listSettingItems[i + 1].gameObject.GetComponent<MultiInputButton>();

			}
			else if (_genericButtons.Length > 0)
			{

				buttonToSelectOnDown = _genericButtons[0].gameObject.GetComponent<MultiInputButton>();
				SetGenericButtonsNavigations(_listSettingItems[i].gameObject.GetComponent<MultiInputButton>());
			}

			if (i - 1 >= 0)
				buttonToSelectOnUp = _listSettingItems[i - 1].gameObject.GetComponent<MultiInputButton>();

			newNavigation.selectOnDown = buttonToSelectOnDown;
			newNavigation.selectOnUp = buttonToSelectOnUp;
			_listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation = newNavigation;
			_listSettingItems[i].SetNavigation(buttonToSelectOnDown, buttonToSelectOnUp);
		}
	}
	void SetGenericButtonsNavigations(MultiInputButton itemUp)
	{
		for (int i = 0; i < _genericButtons.Length; i++)
		{
			Navigation newNavigation = new Navigation();
			newNavigation.mode = Navigation.Mode.Explicit;
			if (i + 1 < _genericButtons.Length)
				newNavigation.selectOnRight = _genericButtons[i + 1].gameObject.GetComponent<MultiInputButton>();
			if (i - 1 > 0)
				newNavigation.selectOnLeft = _genericButtons[i - 1].gameObject.GetComponent<MultiInputButton>();

			newNavigation.selectOnUp = itemUp;


		}

	}
}
