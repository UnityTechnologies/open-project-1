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
	public UnityAction OnCloseCredits;

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private TextAsset _creditsAsset;
	[SerializeField] private TextMeshProUGUI _creditsText = default;
	[SerializeField] private UICreditsRoller _creditsRoller = default;

	[Header("Listening on")]
	[SerializeField] private VoidEventChannelSO _creditsRollEndEvent = default;

	private CreditsList _creditsList;

	private void OnEnable()
	{
		_inputReader.MenuCloseEvent += CloseCreditsScreen;
		SetCreditsScreen();
	}

	private void OnDisable()
	{
		_inputReader.MenuCloseEvent -= CloseCreditsScreen;
	}

	private void SetCreditsScreen()
	{
		_creditsRoller.OnRollingEnded += EndRolling;
		FillCreditsRoller();
		_creditsRoller.StartRolling();
	}

	private void CloseCreditsScreen()
	{
		_creditsRoller.OnRollingEnded -= EndRolling;
		OnCloseCredits.Invoke();
	}

	private void FillCreditsRoller()
	{
		_creditsList = new CreditsList();
		string json = _creditsAsset.text;
		_creditsList = JsonUtility.FromJson<CreditsList>(json);
		SetCreditsText();
	}

	private void SetCreditsText()
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

	private void EndRolling()
	{
		if (_creditsRollEndEvent != null)
			_creditsRollEndEvent.RaiseEvent();
	}
}
