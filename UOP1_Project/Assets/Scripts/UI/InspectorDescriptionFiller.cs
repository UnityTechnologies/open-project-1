using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Components;

public class InspectorDescriptionFiller : MonoBehaviour
{
	[SerializeField]
	private LocalizeStringEvent textDescription;

	[SerializeField]
	private LocalizeStringEvent textName;

	public void FillDescription(Item itemToInspect)
	{
		textName.gameObject.SetActive(true);
		textDescription.gameObject.SetActive(true);


		textName.StringReference = itemToInspect.Name;
		textDescription.StringReference = itemToInspect.Description;

	}

}
