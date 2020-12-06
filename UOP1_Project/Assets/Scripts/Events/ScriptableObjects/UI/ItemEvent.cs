using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "itemEvent", menuName = "UI Event/itemEvent")]
public class ItemEvent : ScriptableObject
{

		public UnityAction<Item> eventRaised;
		public void Raise(Item item)
		{
			if (eventRaised != null)
				eventRaised.Invoke(item);
		}
	
}
