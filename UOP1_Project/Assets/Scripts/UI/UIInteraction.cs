using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
	[SerializeField] private List<InteractionSO> _listInteractions = default;
	[SerializeField] Image _interactionIcon = default;

	public void FillInteractionPanel(InteractionType interactionType)
	{
		if (_listInteractions != null
			&& _listInteractions.Exists(o => o.InteractionType == interactionType))
		{
			Sprite icon = (_listInteractions.Find(o => o.InteractionType == interactionType)).InteractionIcon;
			_interactionIcon.sprite = icon;
		}
	}
}
