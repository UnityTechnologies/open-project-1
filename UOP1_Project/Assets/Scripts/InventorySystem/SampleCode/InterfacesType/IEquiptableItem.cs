using UnityEngine;

namespace InventorySystem
{
    public interface IEquipableItem
    {
        bool IsEquipped();
        void OnEquip(GameObject target);
        void OnUnEquip(GameObject target);
    }
}