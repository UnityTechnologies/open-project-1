using System;
using System.Collections.Generic;
using InventorySystem.Core;
using UnityEngine;

namespace InventorySystem
{
    /// <summary>
    /// MonoGameObject implementation for InventorySystem with item as index only.
    /// Store predefined Items as ScriptableObject also a good solution to replace this. 
    /// </summary>
    public class ItemBag : MonoBehaviour
    {
        public int maxItem = 16;
        
        [SerializeField] [HideInInspector]
        public List<Item> items;

        [Serializable]
        public struct Item
        {
            [ItemID]
            public int id;
            public int count;

            public Item(int id, int count)
            {
                this.id = id;
                this.count = count;
            }
        }

        public bool TryAddItem(int id, int addCount)
        {
            if (addCount <= 0) return false;
            return TryAppendExistItem(id, addCount) || TryAppendNewItem(id,addCount);
        }

        private bool TryAppendExistItem(int id, int addCount)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.id != id)
                    continue;
                items[i] = new Item(id, item.count + addCount);
                return true;
            }
            return false;
        }
        
        private bool TryAppendNewItem(int id, int addCount)
        {
            if (items.Count < maxItem)
            {
                items.Add(new Item(id,addCount));
                return true;
            }
            return false;
        }
        
        public bool TryRemoveItem(int id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.id != id)
                    continue;
                items.RemoveAt(i);
                return true;
            }
            return false;
        }

        public bool TryRemoveItem(int id, int removeCount)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.id != id)
                    continue;
                if (item.count <= removeCount)
                {
                    items.RemoveAt(i);
                    return true;
                }
                else
                    items[i] = new Item(id, item.count - removeCount);
                return true;
            }
            return false;
        }
    }
}