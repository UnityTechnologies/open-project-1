using UnityEngine;

namespace InventorySystem.Sample
{
    public class ToolItemSample : MonoBehaviour, IToolItem
    {
        public void OnInteract()
        {
            // This method should not open GUI canvas or send Event to UI.
            // the method is only a demonstration for case when design demand Item interact with another Item.
            // Or interact special item to open special animation/ Trigger Event in World.
            Debug.Log("Interact item " + gameObject.name);
        }
    }
}