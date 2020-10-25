using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Core
{
    /// <summary>
    /// Contain singular Item List
    /// </summary>
    public class ItemHolder : MonoBehaviour
    {
        [ItemID]
        public List<int> items;
    }
}