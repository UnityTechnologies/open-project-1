using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Character Inventory", order = 51)]
    public class CharacterInventory : ScriptableObject
    {
        [Tooltip("The item stacks possessed by the inventory owner.")]
        [SerializeField]
        private List<ItemStack> _itemStacks = new List<ItemStack>();

        [Tooltip("The maximum number of item stacks that can be in this inventory")]
        private int _maxInventorySize = 10;

        public IReadOnlyList<ItemStack> ItemStacks => _itemStacks;

        public int MaxInventorySize => _maxInventorySize;

        /// <summary>
        /// Adds items of a specific item and quantity. Creates new
        /// stacks as needed. Returns the excess quantity of items
        /// that could not be added, if any.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int Add(InventoryItem item, int quantity)
        {
            for (var i = 0; i < _itemStacks.Count && quantity > 0; i++)
            {
                if (_itemStacks[i].Item != item)
                    continue;

                // Get the items we are adding to the current stack, up to
                // the max stack size of the item
                var delta = Mathf.Min(
                    quantity,
                    item.MaxStackSize - _itemStacks[i].Quantity);
                _itemStacks[i].Quantity += delta;
                quantity -= delta;
            }

            if (quantity == 0)
                return 0;

            if (_itemStacks.Count >= _maxInventorySize)
                return quantity;

            var stacksToAdd = Mathf.CeilToInt((float)quantity / (float)item.MaxStackSize);
            // We can only add a number of stacks up to the maximum inventory size
            stacksToAdd = Math.Min(
                stacksToAdd,
                _maxInventorySize - _itemStacks.Count
            );

            for (var i = 0; i < stacksToAdd; i++)
            {
                // Get the items we are adding to the current stack, up to
                // the max stack size of the item
                var delta = Mathf.Min(
                    quantity,
                    item.MaxStackSize);
                _itemStacks.Add(new ItemStack(item, delta));
                quantity -= delta;
            }

            // Return the overflow
            return quantity;
        }

        /// <summary>
        /// This will attempt to remove a number of the provided item
        /// equal to the quantity, from the end of the inventory in
        /// reverse. It will return the quantity remaining that could
        /// not be removed.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int Remove(InventoryItem item, int quantity)
        {
            if (quantity <= 0)
                return 0;

            // Iterate through the inventory stacks and keep track
            // of how much quantity we need to remove
            var deltas = new List<KeyValuePair<int, int>>();
            for (int i = _itemStacks.Count - 1; i >= 0 && quantity > 0; i--)
            {
                if (_itemStacks[i].Item != item)
                    continue;
                // Get the items we are removing from the current stack, up to
                // the max stack size of the item
                var delta = Mathf.Min(
                    quantity,
                    item.MaxStackSize);
                deltas.Add(new KeyValuePair<int, int>(i, delta));
                quantity -= delta;
            }

            if (deltas.Count == 0)
                return quantity;

            for (var i = deltas.Count - 1; i > 0; i--)
            {
                _itemStacks.RemoveAt(deltas[i].Key);
            }

            Remove(_itemStacks[deltas[0].Key], deltas[0].Value);
            return quantity;
        }

        /// <summary>
        /// This will remove the amount of items from the given stack.
        /// It will return the number of items that could not be removed.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int Remove(ItemStack stack, int quantity)
        {
            if (quantity <= 0)
                return 0;

            var delta = Math.Min(quantity, stack.Quantity);

            stack.Quantity -= delta;
            if (stack.Quantity <= 0)
                _itemStacks.Remove(stack);

            return quantity - delta;
        }

        /// <summary>
        /// Used to swap the indexes of two items.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void Swap(int a, int b)
        {
            var itemA = _itemStacks[a];
            _itemStacks[a] = _itemStacks[b];
            _itemStacks[b] = itemA;
        }

        /// <summary>
        /// Returns the total quantity of the type of item requested.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Total(InventoryItem item)
        {
            var count = 0;
            for (var i = 0; i < _itemStacks.Count; i++)
            {
                if (_itemStacks[i].Item != item)
                    continue;
                count += _itemStacks[i].Quantity;
            }
            return count;
        }

        /// <summary>
        /// Returns true if the inventory has at least the required
        /// value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HasAtLeast(InventoryItem item, int value)
        {
            for (var i = 0; i < _itemStacks.Count; i++)
            {
                if (_itemStacks[i].Item != item)
                    continue;
                if (_itemStacks[i].Quantity >= value)
                    return true;
                value -= _itemStacks[i].Quantity;
            }

            return false;
        }

        private void OnValidate()
        {
            foreach (var stack in _itemStacks)
            {
                stack.Quantity = Mathf.Clamp(stack.Quantity, 1, stack.Item.MaxStackSize);
            }
        }
    }
}