using UnityEngine;

namespace CombatStatemachine
{
    public abstract class CombatTransitionDecision : ScriptableObject
    {
        public abstract bool Decide(CombatStateMachineController _controller);
    }
}

