using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameSettings;

[ExecuteAlways]
public class SettingToText : MonoBehaviour
{
	public TextMeshProUGUI text;
	public GameSetting gameSetting;

	private void LateUpdate()
	{
		text.text = gameSetting.objectValue.ToString();
	}
}
