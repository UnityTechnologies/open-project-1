using System.Collections.Generic;
using UnityEngine;

public class UIDialogueChoicesManager : MonoBehaviour
{
	[SerializeField] private UIDialogueChoiceFiller[] _choiceButtons;

	public void FillChoices(List<Choice> choices)
	{
		if (choices != null)
		{
			int maxCount = Mathf.Max(choices.Count, _choiceButtons.Length);

			for (int i = 0; i < maxCount; i++)
			{
				if (i < _choiceButtons.Length)
				{
					if (i < choices.Count)
					{
						_choiceButtons[i].gameObject.SetActive(true);
						_choiceButtons[i].FillChoice(choices[i], i == 0);
					}
					else
					{
						_choiceButtons[i].gameObject.SetActive(false);
					}
				}
				else
				{
					Debug.LogError("There are more choices than buttons");
				}
			}
		}
	}
}

