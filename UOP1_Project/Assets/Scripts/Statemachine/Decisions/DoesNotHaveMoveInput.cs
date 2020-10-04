
#pragma warning disable CS0649

using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName = CSMUtility.CombatDecisionRoot + "DoesNotHaveMoveInput", fileName = "DoesNotHaveMoveInput")]
    public class DoesNotHaveMoveInput : CombatTransitionDecision
    {
        [Range(0, .5f)]
        [SerializeField] private float m_minThreshold;
        public override bool Decide(CombatStateMachineController _controller)
        {
            Debug.Log("RawMovement " + _controller.HandlerInput.RawMovementInput.sqrMagnitude);
            return _controller.HandlerInput.RawMovementInput.sqrMagnitude <= m_minThreshold;
        }
    }

}
