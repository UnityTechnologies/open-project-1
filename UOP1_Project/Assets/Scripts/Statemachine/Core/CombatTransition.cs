#pragma warning disable CS0649

using UnityEngine;

namespace CombatStatemachine
{

    [System.Serializable]
    public class CombatTransition
    {
        [SerializeField] private CombatTransitionDecision[] m_decisions;
        [SerializeField] private CombatState m_nextState;

        public void EvaluateDecisions(CombatStateMachineController _controller)
        {
            for(int i = 0; i < m_decisions.Length; i++)
            {
                if(m_decisions[i].Decide(_controller))
                {
                    _controller.ChangeState(m_nextState);
                }
            }
        }
        
    }

}
