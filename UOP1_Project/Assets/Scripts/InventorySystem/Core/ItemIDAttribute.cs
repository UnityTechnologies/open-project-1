using System;
using UnityEngine;

namespace InventorySystem.Core
{
    /// <summary>
    /// Show available item exist in game
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ItemIDAttribute : PropertyAttribute
    {
        
    }
}