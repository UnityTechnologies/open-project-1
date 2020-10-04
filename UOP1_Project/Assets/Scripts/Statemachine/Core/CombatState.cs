#pragma warning disable CS0649

using UnityEngine;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatStatePath ,fileName ="CombatState")]
    public class CombatState : ScriptableObject
    {
        #region Inspector Vars
        [SerializeField] private CombatAction[] m_onEnterActions;
        [SerializeField] private CombatAction[] m_onUpdateActions;
        [SerializeField] private CombatAction[] m_onAnimMoveActions;
        [SerializeField] private CombatAction[] m_onExitActions;
        [SerializeField] private CombatAction[] m_onDisableActions;
        [SerializeField] private CombatTransition[] m_transitions;
        #endregion

        #region Public API
        public void OnStateEnter(CombatStateMachineController _controller)
        {
            PerformActions(_controller,m_onEnterActions);
        }
        public void OnStateUpdate(CombatStateMachineController _controller)
        {
            PerformActions(_controller, m_onUpdateActions);
            EvaluateDecisions(_controller);
        }
        public void OnStateAnimatorMove(CombatStateMachineController _controller)
        {
            PerformActions(_controller, m_onAnimMoveActions);
        }
        public void OnStateExit(CombatStateMachineController _controller)
        {
            PerformActions(_controller, m_onExitActions);
        }
        public void OnStateDisable(CombatStateMachineController _controller)
        {
            PerformActions(_controller, m_onDisableActions);
        }
        #endregion

        #region Utility
        private void EvaluateDecisions(CombatStateMachineController _controller)
        {
            for (int i = 0; i < m_transitions.Length; i++)
            {
                m_transitions[i].EvaluateDecisions(_controller);
            }
        }
        private void PerformActions(CombatStateMachineController _controller, CombatAction[] _actions)
        {
            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i].Act(_controller);
            }
        }
        #endregion
    }

}
