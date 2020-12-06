using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventoryInspectorFiller : MonoBehaviour
{
	[SerializeField]
	private InspectorPreviewFiller inspectorPreviewFiller;

	[SerializeField]
	private InspectorDescriptionFiller inspectorDescriptionFiller;


	public void FillItemInspector(Item itemToInspect)
	{

		inspectorPreviewFiller.FillPreview(itemToInspect);
		inspectorDescriptionFiller.FillDescription(itemToInspect);



	}

}

