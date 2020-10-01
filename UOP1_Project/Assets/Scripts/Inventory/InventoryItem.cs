using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 51)]
    public class InventoryItem : ScriptableObject
    {
        [Tooltip("The in-game name of the item.")]
        [SerializeField]
        private string _name = string.Empty;

        [Tooltip("The description of the item.")]
        [TextArea]
        [SerializeField]
        private string _description = string.Empty;

        [Tooltip("The in-game representation of the item.")]
        [SerializeField]
        private GameObject _prefab = default;

        [Tooltip("The inventory screen representation of the item.")]
        [SerializeField]
        private Sprite _icon = default;

        [Tooltip("The maximum quantity of this item the player can possess in a single stack.")]
        [SerializeField]
        private int _maxStackSize = 256;

        [Tooltip("The on-use effects on the item, if any.")]
        [SerializeField]
        private List<ItemEffect> _effects = new List<ItemEffect>();

        [Tooltip("Determines whether or not an item can call the UseItem method.")]
        [SerializeField]
        private bool _isUsable = false;

        public string Name => _name;
        public string Description => _description;
        public GameObject Prefab => _prefab;
        public Sprite Icon => _icon;
        public IReadOnlyList<ItemEffect> Effects => _effects;
        public bool IsUsable => _isUsable;
        public int MaxStackSize => _maxStackSize;

        // This will fire whenever a serialized value is changed in the editor
        private void OnValidate()
        {
            // If the object is cleared, set _name to the name of the object
            _name = !string.IsNullOrWhiteSpace(_name) ? _name : name;
            // Ensure MaxQuantity > 0
            _maxStackSize = Mathf.Max(_maxStackSize, 1);
        }
    }
}