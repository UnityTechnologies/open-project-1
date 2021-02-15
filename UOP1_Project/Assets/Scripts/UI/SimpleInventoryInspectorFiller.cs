using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventoryInspectorFiller : MonoBehaviour
{
	[SerializeField] private InspectorPreviewFiller _inspectorPreviewFiller = default;

	[SerializeField] private InspectorDescriptionFiller _inspectorDescriptionFiller = default;


	public void FillItemInspector(Item itemToInspect)
	{

		_inspectorPreviewFiller.FillPreview(itemToInspect);
		_inspectorDescriptionFiller.FillDescription(itemToInspect);



	}

}

