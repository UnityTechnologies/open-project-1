using UnityEngine;

namespace InventorySystem
{
    public interface IConsumableItem
    {
        void ConsumeBy(GameObject source);
    }
}