using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICredits : MonoBehaviour
{
	private UISettingFieldsFiller _settingFieldsFiller = default;

	public UnityAction closeCreditsAction;
	[SerializeField]
	private InputReader _inputReader = default;

	private void OnEnable()
	{
		_inputReader.menuCloseEvent += CloseCreditsScreen;

	}
	private void OnDisable()
	{
		_inputReader.menuCloseEvent -= CloseCreditsScreen;
	}
	public void SetCreditsScreen()
	{


	}
	public void CloseCreditsScreen()
	{
		closeCreditsAction.Invoke();
	}
}
