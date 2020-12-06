using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "tabTypeEvent", menuName = "UI Event/tabTypeEvent")]
public class TabTypeEvent : ScriptableObject
{

		public UnityAction<InventoryTabType> eventRaised;
		public void Raise(InventoryTabType tabType)
		{
			if (eventRaised != null)
				eventRaised.Invoke(tabType);
		}
	
}
