#pragma warning disable 0649
using JetBrains.Annotations;
using UnityEngine;

namespace InventorySystem.Core
{
    /// <summary>
    /// Item Data contain only basic info and logic GameObject.
    /// Check InventoryEditorUtility on how New Asset was created and added to Database in Editor. 
    /// </summary>
    public class SOItem : ScriptableObject
    {
        [SerializeField]
        private string itemName;

        [SerializeField]
        private Sprite previewSprite;

        /// <summary>
        /// The logic implementation of Item. How it can do will be depend on Implement System.
        /// Note when Project require MemoryManagement, if GameObject become StaticPath/Addressable then some system access GameObject without Instantiate might need refactor.
        /// </summary>
        [SerializeField]
        [CanBeNull]
        private GameObject content;

        [Multiline()]
        [SerializeField]
        private string description;

        public string ItemName => itemName;
        public string Description => description;
        public Sprite PreviewSprite => previewSprite;
        public GameObject Content => content;
    }
}