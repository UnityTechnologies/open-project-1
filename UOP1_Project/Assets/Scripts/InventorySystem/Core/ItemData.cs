using System;
using UnityEngine;

namespace InventorySystem.Core
{
    [Serializable]
    public class ItemData
    {
        public int Index => index;
        public ScriptableObject ItemScriptableObject => itemScriptableObject;

        [SerializeField]
        private int index;

        [SerializeField]
        private ScriptableObject itemScriptableObject;

        public ItemData(int index, ScriptableObject itemScriptableObject)
        {
            this.itemScriptableObject = itemScriptableObject;
            this.index = index;
        }
    }
}