
using UnityEngine;

namespace CombatStatemachine
{
    public abstract class CombatAction : ScriptableObject
    {
        public abstract void Act(CombatStateMachineController _controller);
    }

}
