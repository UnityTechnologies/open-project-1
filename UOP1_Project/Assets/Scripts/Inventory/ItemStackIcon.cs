using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemStackIcon : MonoBehaviour
    {
        [Tooltip("The icon that will be set to the item sprite.")]
        [SerializeField]
        private Image _icon = default;

        [Tooltip("The quantity text.")]
        [SerializeField]
        private Text _quantity = default;

        public ItemStack ItemStack { get; set; }

        public void UpdateQuantity()
        {
            if (ItemStack is null)
            {
                return;
            }

            _icon.sprite = ItemStack.Item.Icon;
            _quantity.text = ItemStack.Quantity.ToString();
        }

        private void Start()
        {
            UpdateQuantity();
        }
    }
}