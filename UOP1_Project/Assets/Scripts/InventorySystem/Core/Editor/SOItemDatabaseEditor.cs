using System.Linq;
using InventorySystem.Core;
using UnityEditor;
using UnityEngine;

namespace InventorySystem.Core.Editor
{
    [CustomEditor(typeof(SOItemDatabase))]
    public class SOItemDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Scan for new Item"))
            {
                ScanForMissingItemInDatabase();
            }
            base.OnInspectorGUI();
        }

        private void ScanForMissingItemInDatabase()
        {
            var foundItemID = AssetDatabase.FindAssets("t:SOItem").Select(s =>
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(s)).GetInstanceID());
            var database = (SOItemDatabase) target;
            var databaseItemID = database.GetAllSOInDatabase().Select(x => x.GetInstanceID());
            var missingItemsID = foundItemID.Except(databaseItemID).ToList();

            if (missingItemsID.Count == 0)
            {
                EditorUtility.DisplayDialog(null, "Found no new Items.\nItem created through AssetMenu don't need to manually added to database", "OK");
                return;
            }

            if (EditorUtility.DisplayDialog("Confirm add new assets", $"Found {missingItemsID.Count} missing Items. Append them?",
                "OK", "Cancel"))
            {
                foreach (var item in missingItemsID)
                {
                    database.AddNewItem((ScriptableObject) EditorUtility.InstanceIDToObject(item));
                }
            }
        }
    }
}