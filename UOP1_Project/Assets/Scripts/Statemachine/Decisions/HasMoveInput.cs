#pragma warning disable CS0649

using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.CombatDecisionRoot+"HasMoveInput",fileName ="HasMoveInput")]
    public class HasMoveInput : CombatTransitionDecision
    {
        [Range(0,.5f)]
        [SerializeField] private float m_minThreshold;
        public override bool Decide(CombatStateMachineController _controller)
        {
            return _controller.HandlerInput.RawMovementInput.sqrMagnitude >= m_minThreshold;
        }
    }

}
