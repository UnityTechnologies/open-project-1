
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditorInternal;


namespace CombatStatemachine
{
    public class StateTransitionListView : VisualElement
    {
        private SerializedObject m_obj;
        private SerializedProperty m_items;
        private string m_listName;

        private ReorderableList m_reorderableList;
        private IMGUIContainer m_container;

        public StateTransitionListView()
        {
            m_obj = null;
            m_items = null;
        }
        public StateTransitionListView(SerializedObject _owner, SerializedProperty _list, string _listName)
        {
            m_obj = _owner;
            m_items = _list;
            m_listName = _listName;

            m_container = new IMGUIContainer(() => OnGUIHandler()) { name = "TransitionListContainer" };
            Add(m_container);
        }

        private void OnGUIHandler()
        {
            if (m_reorderableList == null)
            {
                CreateReorderableList();
                AddListCallBacks();
            }
            m_reorderableList.DoLayoutList();
        }
        private void CreateReorderableList()
        {
            m_reorderableList = new ReorderableList(m_obj, m_items, true, true, true, true);
        }
        private void AddListCallBacks()
        {
            m_reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                var labelRect = new Rect(rect.x, rect.y, rect.width - 10, rect.height);
                EditorGUI.LabelField(labelRect, m_listName);
            };
            m_reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                EditorGUI.BeginChangeCheck();
               
                SerializedProperty element = m_reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(new Rect(rect.x += 10, rect.y, Screen.width * .8f, EditorGUIUtility.singleLineHeight), element, new GUIContent("Transition"), true);
                
                if(EditorGUI.EndChangeCheck())
                {
                    m_obj.ApplyModifiedProperties();
                }
                
            };
            m_reorderableList.elementHeightCallback = (int index) =>
            {
                float proertyHeight = EditorGUI.GetPropertyHeight(m_reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

                float spacing = EditorGUIUtility.singleLineHeight / 2;

                return proertyHeight + spacing;
            };
            m_reorderableList.onChangedCallback = (ReorderableList _reorderList) =>
            {
                m_obj.ApplyModifiedProperties();
            };
        }
    }

}
