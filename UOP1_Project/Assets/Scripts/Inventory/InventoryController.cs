using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Tooltip("The character's inventory.")]
        [SerializeField]
        private CharacterInventory _inventory = default;

        [Tooltip("The target of the character's item use.")]
        [SerializeField]
        private GameObject _itemTarget = default;

        [Tooltip("Invoked whenever the inventory is updated.")]
        public UnityEvent InventoryUpdated = new UnityEvent();

        public CharacterInventory Inventory => _inventory;
        public GameObject ItemTarget => _itemTarget;

        public void PickupItem(ItemPickup pickup)
        {
            Debug.Log($"{name} picked up {pickup.name}");
            var overflow = _inventory.Add(pickup.Item, pickup.Quantity);
            InventoryUpdated.Invoke();
            pickup.Quantity = overflow;
            pickup.Pickup();
        }

        public void UseItem(InventoryItem item)
        {
            if (!item.IsUsable)
                return;

            if (!ItemTarget)
                return;

            if (!_inventory.HasAtLeast(item, 1))
                return;

            _inventory.Remove(item, 1);

            for (var i = 0; i < item.Effects.Count; i++)
            {
                item.Effects[i].ApplyEffect(ItemTarget);
            }

            InventoryUpdated.Invoke();
        }

        public void SetTarget(GameObject target)
        {
            _itemTarget = target;
        }

        private void Start()
        {
            // If we don't have a target, go ahead and assume
            // this gameObject is the target
            if (!ItemTarget)
            {
                SetTarget(gameObject);
            }
        }
    }
}