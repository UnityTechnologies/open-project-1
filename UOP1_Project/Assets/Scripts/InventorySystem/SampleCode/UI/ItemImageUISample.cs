#pragma warning disable 0649
using System;
using InventorySystem.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Sample
{
    public class ItemImageUISample : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private  TextMeshProUGUI countText;
        [SerializeField] private  Image previewTexture;
        public Action onClick;
        [SerializeField]
        private Button button;

        private void Awake()
        {
            button.onClick.AddListener(()=>onClick?.Invoke());
        }

        public void SetupItem(SOItem data,int count)
        {
            nameText.text = data.ItemName;
            previewTexture.sprite = data.PreviewSprite;
            countText.transform.parent.gameObject.SetActive(count > 1);
            countText.text = count > 0 ? count.ToString() : null;
        }
    }
}