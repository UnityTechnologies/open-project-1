using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionManager : MonoBehaviour
{
	[SerializeField]
	private List<InteractionSO> listInteractions;
	[SerializeField]
	private UIInteractionItemFiller interactionItem;

	public void FillInteractionPanel(InteractionType interactionType)
	{
		if ((listInteractions != null) && (interactionItem != null))
			if (listInteractions.Exists(o => o.InteractionType == interactionType))

			{
				interactionItem.FillInteractionPanel(listInteractions.Find(o => o.InteractionType == interactionType));

			}
	}

}
