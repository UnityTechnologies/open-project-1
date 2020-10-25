using InventorySystem.Core;
using UnityEngine;

namespace InventorySystem.Sample
{
    /// <summary>
    /// Sample Inventory Manager.
    /// This class should be the single one that have access to ItemDatabase
    /// Handle most of logic required by other system like Spawn Item or acquire Item info
    /// </summary>
    public class InventoryManagerSample : MonoBehaviour
    {
        public SOItemDatabase database;
        
        public SOItem GetItem(int index)
        {
            var scriptableObject = database.GetItem(index);
            if (scriptableObject is SOItem item)
                return item;
            Debug.LogError("Missing item ID:" + index);
            return null;
        }

        public void UseConsumeItemOnTarget(int itemID, GameObject target)
        {
            var soItem = GetItem(itemID);
            if (soItem.Prefab != null)
            {
                var item = Instantiate(soItem.Prefab, target.transform);
                item.GetComponent<IConsumableItem>().ConsumeBy(target);
                Destroy(item);
            }
        }
        
        public void EquipItemOnTarget(int itemID, GameObject target)
        {
            var soItem = GetItem(itemID);
            if (soItem.Prefab != null)
            {
                var item = Instantiate(soItem.Prefab, target.transform);
                item.GetComponent<IEquipableItem>().OnEquip(target);
                Destroy(item);
            }
        }
    }
}