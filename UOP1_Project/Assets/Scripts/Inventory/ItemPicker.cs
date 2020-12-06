using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{

	[SerializeField]
	ItemEvent AddItemEvent;

	public void PickItem(Item item)
	{
		if(AddItemEvent!=null)
		AddItemEvent.Raise(item); 
	}
}
