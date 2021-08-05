using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetButtonNavigation : MonoBehaviour
{
	UIGenericButton[] genericButtons;
	UISettingItemFiller[] listSettingItems;
	private void Start()
	{
		listSettingItems = GetComponentsInChildren<UISettingItemFiller>();

		genericButtons = GetComponentsInChildren<UIGenericButton>();
		MultiInputButton buttonToSelectOnDown = default;
		MultiInputButton buttonToSelectOnUp = default;
		for (int i = 0; i < listSettingItems.Length; i++)
		{
			Navigation newNavigation = new Navigation();

			newNavigation.mode = Navigation.Mode.Explicit;

			newNavigation.selectOnLeft = listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation.selectOnLeft;
			newNavigation.selectOnRight = listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation.selectOnRight;

			if (i + 1 < listSettingItems.Length)
			{
				buttonToSelectOnDown = listSettingItems[i + 1].gameObject.GetComponent<MultiInputButton>();

			}
			else if (genericButtons.Length > 0)
			{

				buttonToSelectOnDown = genericButtons[0].gameObject.GetComponent<MultiInputButton>();
				SetGenericButtonsNavigations(listSettingItems[i].gameObject.GetComponent<MultiInputButton>());
			}

			if (i - 1 >= 0)
				buttonToSelectOnUp = listSettingItems[i - 1].gameObject.GetComponent<MultiInputButton>();

			newNavigation.selectOnDown = buttonToSelectOnDown;
			newNavigation.selectOnUp = buttonToSelectOnUp;
			listSettingItems[i].gameObject.GetComponent<MultiInputButton>().navigation = newNavigation;
			listSettingItems[i].SetNavigation(buttonToSelectOnDown, buttonToSelectOnUp);
		}
	}
	private void OnEnable()
	{
		if (listSettingItems == null)
		{
			listSettingItems = GetComponentsInChildren<UISettingItemFiller>();
		}
		if (listSettingItems.Length > 0)
			if (listSettingItems[0].GetComponent<MultiInputButton>() != null)
				//select first item
				listSettingItems[0].GetComponent<MultiInputButton>().Select();
	}
	void SetGenericButtonsNavigations(MultiInputButton itemUp)
	{
		for (int i = 0; i < genericButtons.Length; i++)
		{
			Navigation newNavigation = new Navigation();
			newNavigation.mode = Navigation.Mode.Explicit;
			if (i + 1 < genericButtons.Length)
				newNavigation.selectOnRight = genericButtons[i + 1].gameObject.GetComponent<MultiInputButton>();
			if (i - 1 > 0)
				newNavigation.selectOnLeft = genericButtons[i - 1].gameObject.GetComponent<MultiInputButton>();

			newNavigation.selectOnUp = itemUp;


		}

	}
}
