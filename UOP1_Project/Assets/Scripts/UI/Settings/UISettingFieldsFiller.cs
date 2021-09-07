using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UISettingFieldsFiller : MonoBehaviour
{
	[SerializeField] private UISettingItemFiller[] _settingfieldsList = default;

	public void FillFields(List<SettingField> settingItems)
	{
		for (int i = 0; i < _settingfieldsList.Length; i++)
		{
			if (i < settingItems.Count)
			{
				SetField(settingItems[i], _settingfieldsList[i]);
				_settingfieldsList[i].gameObject.SetActive(true);
			}
			else
			{
				_settingfieldsList[i].gameObject.SetActive(false);
			}
		}

	}

	public void SetField(SettingField field, UISettingItemFiller uiField)
	{
		int paginationCount = 0;
		int selectedPaginationIndex = 0;
		string selectedOption = default;
		LocalizedString fieldTitle = field.title;
		SettingFieldType fieldType = field.settingFieldType;

		switch (field.settingFieldType)
		{
			case SettingFieldType.Language:
				paginationCount = LocalizationSettings.AvailableLocales.Locales.Count;
				selectedPaginationIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(o => o == LocalizationSettings.SelectedLocale);
				selectedOption = LocalizationSettings.SelectedLocale.LocaleName;
				break;
			case SettingFieldType.AntiAliasing:

				break;
			case SettingFieldType.FullScreen:
				selectedPaginationIndex = IsFullscreen();
				paginationCount = 2;
				if (Screen.fullScreen)
					selectedOption = "On";
				else
					selectedOption = "Off";
				break;
			case SettingFieldType.ShadowDistance:

				break;
			case SettingFieldType.Resolution:

				break;
			case SettingFieldType.ShadowQuality:

				break;
			case SettingFieldType.Volume_Music:
			case SettingFieldType.Volume_SFx:
				paginationCount = 10;
				selectedPaginationIndex = 5;
				selectedOption = "5";
				break;



		}
		uiField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);


	}
	
	int IsFullscreen()
	{
		if (Screen.fullScreen)
		{
			return 0;
		}
		else
		{
			return 1;
		}

	}
}
