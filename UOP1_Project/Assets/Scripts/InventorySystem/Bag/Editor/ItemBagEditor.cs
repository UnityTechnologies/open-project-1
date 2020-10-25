using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace InventorySystem.Core.Editor
{
    [CustomEditor(typeof(ItemBag))]
    public class ItemBagEditor : UnityEditor.Editor
    {
        ReorderableList sortableList;

        private void OnEnable()
        {
            var items = serializedObject.FindProperty("items");
            sortableList = new ReorderableList(serializedObject, items, true, true, true, true);
            sortableList.drawElementCallback = DrawListItems;
            sortableList.drawHeaderCallback = DrawHeader;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Items");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            sortableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var labelWidth = 44f;
            var countInputWidth = 40f;
            SerializedProperty element = sortableList.serializedProperty.GetArrayElementAtIndex(index);

            rect.width -= countInputWidth + labelWidth;
            EditorGUI.PropertyField(
                rect,
                element.FindPropertyRelative("id"),
                GUIContent.none
            );
            
            rect.x += rect.width;
            rect.width = labelWidth;
            EditorGUI.LabelField(
                new Rect(rect.x + rect.width - countInputWidth, rect.y, countInputWidth, EditorGUIUtility.singleLineHeight),
                new GUIContent("Count"));
            
            rect.x += rect.width;
            rect.width = countInputWidth;
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - countInputWidth, rect.y, countInputWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("count"),
                GUIContent.none
            );
        }
    }
}