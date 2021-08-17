using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class CreditsList
{
	public List<ContributerProfile> Contributors = new List<ContributerProfile>();

}
[System.Serializable]
public class ContributerProfile
{
	public string Name;
	public string Contribution;
	public override string ToString()
	{
		return Name + " - " + Contribution;
	}

}
public class UICredits : MonoBehaviour
{
	public UnityAction closeCreditsAction;
	[SerializeField]
	private InputReader _inputReader = default;
	[SerializeField]
	private string _creditsFilename = default;
	[SerializeField]
	private TextMeshProUGUI _creditsText = default;
	[SerializeField]
	private VoidEventChannelSO _creditsRollEndEvent = default;
	[SerializeField]
	private UICreditsRoller _creditsRoller = default;

	private CreditsList _creditsList = default;
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
		_creditsRoller.rollingEnded += EndRolling;
		FillCreditsRoller();
		_creditsRoller.StartRolling();


	}
	public void CloseCreditsScreen()
	{
		_creditsRoller.rollingEnded -= EndRolling;
		closeCreditsAction.Invoke();
	}
	public void FillCreditsRoller()
	{
		_creditsList = new CreditsList();
		TextAsset creditsList = Resources.Load(_creditsFilename) as TextAsset;
		string json = creditsList.text;
		_creditsList = JsonUtility.FromJson<CreditsList>(json);
		SetCreditsText();
	}
	public void SetCreditsText()
	{
		string creditsText = "";
		for (int i = 0; i < _creditsList.Contributors.Count; i++)
		{
			if (i == 0)
				creditsText = creditsText + _creditsList.Contributors[i].ToString();
			else
			{
				creditsText = creditsText + "\n" + _creditsList.Contributors[i].ToString();

			}
		}
		_creditsText.text = creditsText;

	}
	public void EndRolling()
	{
		if (_creditsRollEndEvent != null)
			_creditsRollEndEvent.RaiseEvent();
	}
}
