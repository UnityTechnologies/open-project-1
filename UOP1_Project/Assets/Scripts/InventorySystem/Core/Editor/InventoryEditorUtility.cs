using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace InventorySystem.Core.Editor
{
    public static class InventoryEditorUtility
    {
        private static SOItemDatabase _instance;
        // Do not Move ItemDatabase to Resources. Or set Database to static place until the Project decide on Memory management.
        public static SOItemDatabase ItemDatabaseInstance
        {
            get
            {
                if (_instance == null)
                {
                    var results  = AssetDatabase.FindAssets("t:SOItemDatabase");
                    if(results.Length == 0)
                        throw new Exception("No SOItemDatabase file found in project");
                    if(results.Length > 2)
                        throw new Exception("Found more than 2 SOItemDatabase. By First Design, it suppose to only have 1 center item database.");
                    _instance = AssetDatabase.LoadAssetAtPath<SOItemDatabase>(AssetDatabase.GUIDToAssetPath(results[0]));
                }
                return _instance;
            }
        }

        // For item that created by Duplicated. That will be handle by Unity Editor Test Case.
        // And programmer will have to manually scan new item into Database.
        [MenuItem("Assets/Create/ScriptableObjects/New Item", priority = 1)]
        public static void CreateNewItem()
        {
            SOItem asset = ScriptableObject.CreateInstance<SOItem> ();
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (path == "") 
            {
                path = "Assets";
            } 
            else if (Path.GetExtension (path) != "") 
            {
                path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
            }
 
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New Item".ToString() + ".asset");
 
            AssetDatabase.CreateAsset (asset, assetPathAndName);
            Selection.activeInstanceID = asset.GetInstanceID();
            ItemDatabaseInstance.AddNewItem(asset);
        }
    }
}