using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Inventory
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        [Tooltip("The inventory item associated with this pickup.")]
        [SerializeField]
        private InventoryItem _item = default;

        [Tooltip("The quantity of items associated with this pickup.")]
        [SerializeField]
        private int _quantity = 1;

        [Tooltip("Invoked when the item is picked up.")]
        public UnityEvent ItemPickedUp = new UnityEvent();

        public InventoryItem Item => _item;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = Mathf.Clamp(value, 0, _item.MaxStackSize);
            }
        }

        public void Pickup()
        {
            ItemPickedUp.Invoke();
            // TODO: Verify we want to destroy this here - do we want to check if quantity > 0?
            Destroy(gameObject);
        }

        private void Awake()
        {
            Assert.IsNotNull(_item);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO: Probably want this done via Interact instead of collision
            if (other.gameObject.TryGetComponent<InventoryController>(out var inventory))
            {
                inventory.PickupItem(this);
            }
        }

        private void OnValidate()
        {
            if (Item)
            {
                _quantity = Mathf.Clamp(_quantity, 1, Item.MaxStackSize);
            }
        }
    }
}