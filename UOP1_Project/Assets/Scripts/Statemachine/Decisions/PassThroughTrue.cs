using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatDecisionRoot+"PassThroughTrue",fileName ="PassThroughTrue")]
    public class PassThroughTrue : CombatTransitionDecision
    {
        public override bool Decide(CombatStateMachineController _controller)
        {
            return true;
        }
    }

}
