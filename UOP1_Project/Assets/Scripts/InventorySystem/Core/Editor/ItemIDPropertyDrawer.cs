using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
// ReSharper disable Unity.NoNullPropagation

namespace InventorySystem.Core.Editor
{
    [CustomPropertyDrawer(typeof(ItemIDAttribute))]
    public class ItemIDPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            rect = EditorGUI.PrefixLabel(rect, label);

            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(rect, "Error: attribute can be applied only to Int type");
                return;
            }

            var database = InventoryEditorUtility.ItemDatabaseInstance;
            
            var field = typeof(SOItemDatabase).GetField("itemDatabase", BindingFlags.Instance | BindingFlags.NonPublic);
            var itemList = (List<ItemData>) field.GetValue(database);

            var currentItemIndex = property.intValue;
            var currentItem = database.GetItem(currentItemIndex);
            var currentItemGUI = new GUIContent($"{currentItemIndex}: {currentItem?.name}");
            
            // Rect for 2nd Button to highlight selected asset
            rect.width -= 35;
            var highlightRect = rect;
            highlightRect.x += rect.width;
            highlightRect.width = 35;
            
            if (GUI.Button(rect, currentItemGUI, EditorStyles.popup))
            {
                var options = itemList.Select(item=>$"{item.Index}: {item.ItemScriptableObject.name}").ToArray();
                SearchablePopup.Show(rect, currentItemIndex, options, chooseIndex =>
                {
                    var chooseItem = itemList[chooseIndex];
                    property.intValue = chooseItem.Index;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            
            if (GUI.Button(highlightRect, new GUIContent("Edit"), EditorStyles.miniButton))
            {
                Selection.activeInstanceID = currentItem.GetInstanceID();
            }

        }
    }
}
