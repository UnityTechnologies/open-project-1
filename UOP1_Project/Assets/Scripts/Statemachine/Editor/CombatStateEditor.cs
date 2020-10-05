using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CombatStatemachine
{
    [CustomEditor(typeof(CombatState))]
    public class CombatStateEditor : Editor
    {
        private VisualElement m_rootElement;
        private VisualTreeAsset m_visualTree;
        private SerializedObject m_obj;

        private void OnEnable()
        {
            m_obj = serializedObject;
            m_rootElement = new VisualElement();

        }
        public override VisualElement CreateInspectorGUI()
        {
            m_rootElement.Clear();

            m_rootElement.Add(new PropertyField(m_obj.FindProperty("m_combatAnim"), "Combat Animation"));
            m_rootElement.Add(new ReorderableListViewElement(m_obj, m_obj.FindProperty("m_onEnterActions"), "On Enter Actions"));
            m_rootElement.Add(new ReorderableListViewElement(m_obj, m_obj.FindProperty("m_onUpdateActions"), "On Update Actions"));
            m_rootElement.Add(new ReorderableListViewElement(m_obj, m_obj.FindProperty("m_onAnimMoveActions"), "On AnimatorMoveActions Actions"));
            m_rootElement.Add(new ReorderableListViewElement(m_obj, m_obj.FindProperty("m_onExitActions"), "On Exit Actions"));
            m_rootElement.Add(new StateTransitionListView(m_obj, m_obj.FindProperty("m_transitions"), "Transitions"));

            m_obj.ApplyModifiedProperties();
            return m_rootElement;
        }
    }

}
