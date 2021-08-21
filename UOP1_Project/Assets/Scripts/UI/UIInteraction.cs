using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
	[SerializeField]
	private List<InteractionSO> _listInteractions = default;

	[SerializeField] Image _interactionIcon = default;
	public void FillInteractionPanel(InteractionType interactionType)
	{
		if (_listInteractions != null)
			if (_listInteractions.Exists(o => o.InteractionType == interactionType))

			{
				FillInteractionPanel(_listInteractions.Find(o => o.InteractionType == interactionType));

			}
	}


	public void FillInteractionPanel(InteractionSO interactionItem)
	{
		_interactionIcon.sprite = interactionItem.InteractionIcon;

	}

}
