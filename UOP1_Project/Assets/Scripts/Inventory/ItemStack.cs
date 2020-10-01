using System;

namespace Inventory
{
    [Serializable]
    public class ItemStack
    {
        public InventoryItem Item;
        public int Quantity;

        public ItemStack() { }

        public ItemStack(InventoryItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}