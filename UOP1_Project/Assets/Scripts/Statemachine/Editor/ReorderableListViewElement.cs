
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditorInternal;

namespace CombatStatemachine
{
    public class ReorderableListViewElement : VisualElement
    {
        private SerializedObject m_ownerObject;
        private SerializedProperty m_items;
        private string m_listName;

        private ReorderableList m_reorderableList;
        private IMGUIContainer m_container;

        #region Internal Classes
        public new class UxmlFactory : UxmlFactory<ReorderableListViewElement>
        {
            public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
            {
                return base.Create(bag, cc);
            }
        }

        #endregion

        public ReorderableListViewElement()
        {
            m_ownerObject = null;
            m_items = null;
        }
        public ReorderableListViewElement(SerializedObject _owner, SerializedProperty _items, string _listName)
        {
            m_ownerObject = _owner;
            m_items = _items;
            m_listName = _listName;

            m_container = new IMGUIContainer(() => OnGUIHandler()) { name = "ListContainer" };
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
            m_reorderableList = new ReorderableList(m_ownerObject, m_items, true, true, true, true);
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

                EditorGUI.PropertyField(rect, m_items.GetArrayElementAtIndex(index), GUIContent.none);

                if(EditorGUI.EndChangeCheck())
                {
                    m_ownerObject.ApplyModifiedProperties();
                }
            };
            m_reorderableList.elementHeightCallback = (int index) => 
            {
               
                float propertyHeight = EditorGUI.GetPropertyHeight(m_reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);
                float spacing = EditorGUIUtility.singleLineHeight / 2;


                return propertyHeight + spacing;
            };

           
            m_reorderableList.onAddCallback = (ReorderableList list) =>
            {
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;
                m_ownerObject.ApplyModifiedProperties();

            };
            m_reorderableList.onChangedCallback = (ReorderableList _reorderList) =>
            {
                m_ownerObject.ApplyModifiedProperties();
            };
            m_reorderableList.onReorderCallback = (ReorderableList list) =>
            {
                m_ownerObject.ApplyModifiedProperties();
            };
            m_reorderableList.onSelectCallback = (ReorderableList list) =>
            {
                m_ownerObject.ApplyModifiedProperties();
            };
            

        }
    }

}
