#pragma warning disable CS0649

using UnityEngine;


namespace CombatStatemachine
{
    [System.Serializable]
    public class MovementHandlerData
    {
        [Tooltip("Horizontal XZ plane speed multiplier")]
        [SerializeField] private float speed = 8f;
        [Tooltip("Smoothing for rotating the character to their movement direction")]
        [SerializeField] private float turnSmoothTime = 0.2f;
        [Tooltip("General multiplier for gravity (affects jump and freefall)")]
        [SerializeField] private float gravityMultiplier = 5f;
        [Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
        [SerializeField] private float initialJumpForce = 10f;
        [Tooltip("How long can the player hold the jump button")]
        [SerializeField] private float jumpInputDuration = .4f;
        [Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")]
        [SerializeField] private float gravityComebackMultiplier = 15f;
        [Tooltip("The maximum speed reached when falling (in units/frame)")]
        [SerializeField] private float maxFallSpeed = 50f;
        [Tooltip("Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")]
        [SerializeField] private float gravityDivider = .6f;

        public float Speed { get { return speed; } }
        public float TurnSmoothTime { get { return turnSmoothTime; } }
        public float GravityMultiplier { get { return gravityMultiplier; } }
        public float InitialJumpForce { get { return initialJumpForce; } }
        public float GravityCombackMultiplier { get { return gravityComebackMultiplier; } }
        public float MaxFallSpeed { get { return maxFallSpeed; } }
        public float GravityDivider { get { return gravityDivider; } }
        public float JumpInputDuration { get { return jumpInputDuration; } }

    }

}
