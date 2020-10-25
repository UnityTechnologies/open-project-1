using InventorySystem.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable Unity.NoNullPropagation
namespace InventorySystem.Sample
{
    /// <summary>
    /// This is only sample code to show how to implement InventorySystem.
    /// Do not extend this class to make GUI for real system.
    /// All GUI implemented here are just bad practices for demo purpose.
    /// In Real case, GUI shouldn't handle logic on Inventory like this.
    /// It should go through manager class or Event. Ex: Attach Button to ManagerClass. UI only update new info
    /// Until project design become more specific, this is just a simple demo for people discuss on framework for first iteration
    /// </summary>
    public class InventoryViewUISample : MonoBehaviour
    {
        public InventoryManagerSample inventoryManager;

        public ItemBag itemBag;
        public Canvas canvas;

        public Transform rootScrollContent;
        public ItemImageUISample scrollItemImagePrefab;

        public TextMeshProUGUI descriptionText;

        public Button eatItemButton;
        public Button craftItemButton;
        public Button equipItemButton;

        [ItemID]
        public int lastSelectedItem;

        public Transform rootPreviewTransform;

        private void OnEnable()
        {
            if (!ReferenceEquals(itemBag, null))
                ShowItemBag(itemBag);
        }

        public void ShowItemBag(ItemBag bag)
        {
            this.itemBag = bag;
            canvas.enabled = false;
            ResetView();
            canvas.enabled = true;
        }

        private void ResetView()
        {
            DestroyChildrenTransform(rootScrollContent);
            foreach (var bagItem in itemBag.items)
            {
                var item = Instantiate(scrollItemImagePrefab, rootScrollContent);
                var itemData = inventoryManager.GetItem(bagItem.id);
                item.SetupItem(itemData, bagItem.count);
                item.onClick = () => OnClickItem(bagItem);
            }

            HideAllButton();
            descriptionText.text = null;
            lastSelectedItem = -1;
            DestroyChildrenTransform(rootPreviewTransform);
        }

        private void HideAllButton()
        {
            eatItemButton.gameObject.SetActive(false);
            equipItemButton.gameObject.SetActive(false);
            craftItemButton.gameObject.SetActive(false);
        }

        private void OnClickItem(ItemBag.Item item)
        {
            lastSelectedItem = item.id;
            var itemData = inventoryManager.GetItem(item.id);
            ShowItemInfo(itemData);
            ShowPreviewModelItem(itemData);

            ShowButtonIfHaveItemType<IConsumableItem>(itemData, eatItemButton);
            ShowButtonIfHaveItemType<IEquipableItem>(itemData, equipItemButton);
            ShowButtonIfHaveItemType<IToolItem>(itemData, craftItemButton);
        }

        private void ShowItemInfo(SOItem itemData)
        {
            descriptionText.text = $"{itemData.ItemName}\n{itemData.Description}";
        }

        void ShowPreviewModelItem(SOItem itemData)
        {
            DestroyChildrenTransform(rootPreviewTransform);
            if (itemData.Prefab == null)
                return;
            Instantiate(itemData.Prefab, rootPreviewTransform);
        }

        private void ShowButtonIfHaveItemType<T>(SOItem item, Button button) where T : class
        {
            if (item.Prefab == null)
            {
                return;
            }

            var go = item.Prefab.GetComponent<T>();
            button.gameObject.SetActive(go != null);
        }

        private void DestroyChildrenTransform(Transform trans)
        {
            int i = 0;
            GameObject[] allChildren = new GameObject[trans.childCount];
            foreach (Transform child in trans)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                Destroy(child);
            }
        }

        // UI Action Method

        public void ConsumeSelectedItem()
        {
            if (itemBag.TryRemoveItem(lastSelectedItem, 1))
            {
                inventoryManager.UseConsumeItemOnTarget(lastSelectedItem, itemBag.gameObject);
                ResetView();
            }
        }

        public void EquipSelectedItem()
        {
            if (itemBag.TryRemoveItem(lastSelectedItem, 1))
            {
                inventoryManager.EquipItemOnTarget(lastSelectedItem, itemBag.gameObject);
                ResetView();
            }
        }

        public void InteractSelectedItem()
        {
            if (itemBag.TryRemoveItem(lastSelectedItem, 1))
                ResetView();
        }
    }
}