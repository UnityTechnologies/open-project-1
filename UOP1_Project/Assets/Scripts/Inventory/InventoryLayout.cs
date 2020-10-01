using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InventoryLayout : MonoBehaviour
    {
        [Tooltip("The character inventory being displayed.")]
        [SerializeField]
        private InventoryController _inventory = default;

        [Tooltip("The prefab used to instantiate item stack icons.")]
        [SerializeField]
        private ItemStackIcon _itemStackIconPrefab = default;

        private List<ItemStackIcon> _icons = new List<ItemStackIcon>();

        public void UpdateInventory()
        {
            for (var i = 0; i < _icons.Count; i++)
            {
                if (i < _inventory.Inventory.ItemStacks.Count)
                {
                    _icons[i].ItemStack = _inventory.Inventory.ItemStacks[i];
                    _icons[i].gameObject.SetActive(true);
                    _icons[i].UpdateQuantity();
                }
                else
                {
                    _icons[i].gameObject.SetActive(false);
                }
            }
        }

        private void Awake()
        {
            _icons = GetComponentsInChildren<ItemStackIcon>().ToList();
            if (_icons.Count < _inventory.Inventory.MaxInventorySize)
            {
                for (var i = _icons.Count; i < _inventory.Inventory.MaxInventorySize; i++)
                {
                    _icons.Add(Instantiate<ItemStackIcon>(_itemStackIconPrefab, transform));
                }
            }
            else if (_icons.Count > _inventory.Inventory.MaxInventorySize)
            {
                for (var i = _icons.Count; i > _inventory.Inventory.MaxInventorySize; i--)
                {
                    _icons.RemoveAt(_icons.Count - 1);
                    Destroy(_icons[i].gameObject);
                }
            }

            UpdateInventory();
        }
    }
}