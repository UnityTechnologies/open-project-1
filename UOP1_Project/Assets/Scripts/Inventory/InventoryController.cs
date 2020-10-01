using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemInstance>(out var itemInstance))
        {
            _inventory.Add(itemInstance.Item);
            Destroy(itemInstance);
        }
    }
}
