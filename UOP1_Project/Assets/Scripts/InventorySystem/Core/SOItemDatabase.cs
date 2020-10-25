#pragma warning disable 0649
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace InventorySystem.Core
{
    /// <summary>
    /// Single Center Database hold reference for all Item exist in game.
    /// </summary>
    public class SOItemDatabase : ScriptableObject
    {
        // New item will use this index to prevent duplicate
        [SerializeField]
        [HideInInspector]
        private int lastIndex;

        [SerializeField]
        private List<ItemData> itemDatabase;
        
        // Intentionally not convert to Quick Search Dictionary due to Database might get update in Playmode.
        // The optimization search performance is ignorable
        [CanBeNull]
        public ScriptableObject GetItem(int index)
        {
            foreach (var item in itemDatabase)
            {
                if (item.Index == index)
                    return item.ItemScriptableObject;
            }

            return null;
        }

        public void AddNewItem([NotNull] ScriptableObject scriptableObject)
        {
            if (scriptableObject == null)
                throw new ArgumentNullException(nameof(scriptableObject));
            itemDatabase.Add(new ItemData(++lastIndex, scriptableObject));
        }
    }
}