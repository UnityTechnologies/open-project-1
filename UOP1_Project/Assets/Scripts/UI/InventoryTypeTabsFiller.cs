using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTypeTabsFiller : MonoBehaviour
{


	[SerializeField]
	private List<InventoryTypeTabFiller> instantiatedGameObjects;

	bool canDisableLayout = false; 

	public void FillTabs(List<InventoryTabType> typesList, InventoryTabType selectedType, TabEventChannelSO changeTabEvent)
	{

		if (instantiatedGameObjects == null)
			instantiatedGameObjects = new List<InventoryTypeTabFiller>();

		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;


		int maxCount = Mathf.Max(typesList.Count, instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < typesList.Count)
			{
				if (i >= instantiatedGameObjects.Count)
				{
					Debug.Log("Maximum tabs reached");
				}
				bool isSelected = typesList[i] == selectedType;
				//fill
				instantiatedGameObjects[i].fillTab(typesList[i], isSelected);
				instantiatedGameObjects[i].gameObject.SetActive(true);

			}
			else if (i < instantiatedGameObjects.Count)
			{
				//Desactive
				instantiatedGameObjects[i].gameObject.SetActive(false);
			}

		}
		if (isActiveAndEnabled) // check if the game object is active and enabled so that we could start the coroutine. 
		{
			StartCoroutine(waitBeforeDesactiveLayout());

		}
		else // if the game object is inactive, disabling the layout will happen on onEnable 
		{
			canDisableLayout = true;
		}

	}
	IEnumerator waitBeforeDesactiveLayout()
	{


		yield return new WaitForSeconds(1);
		//disable layout group after layout calculation
		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
			canDisableLayout = false;
		}
	}

	private void OnEnable()
	{
		if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && canDisableLayout)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
			canDisableLayout = false;
		}
	}

}
