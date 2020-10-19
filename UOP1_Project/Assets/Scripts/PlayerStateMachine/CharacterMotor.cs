using System;
using UnityEngine;

namespace PlayerStateMachine
{
    public class CharacterMotor : MonoBehaviour
    {
        private CharacterController characterController;

        [Tooltip("Horizontal XZ plane speed multiplier")]
        public float speed = 8f;

        [Tooltip("Smoothing for rotating the character to their movement direction")]
        public float turnSmoothTime = 0.2f;

        [Tooltip("General multiplier for gravity (affects jump and freefall)")]
        public float gravityMultiplier = 5f;

        [Tooltip(
            "The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
        public float initialJumpForce = 10f;

        [Tooltip("How long can the player hold the jump button")]
        public float jumpInputDuration = .4f;

        [Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")]
        public float gravityComebackMultiplier = 15f;

        [Tooltip("The maximum speed reached when falling (in units/frame)")]
        public float maxFallSpeed = 50f;

        [Tooltip(
            "Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")]
        public float gravityDivider = .6f;

        public float
            gravityContributionMultiplier =
                0f; //The factor which determines how much gravity is affecting verticalMovement

        public bool isJumping = false; //If true, a jump is in effect and the player is holding the jump button
        public float jumpBeginTime = -Mathf.Infinity; //Time of the last jump

        public float
            turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction

        public float
            verticalMovement =
                0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier

        public Vector3 inputVector; //Initial input horizontal movement (y == 0f)
        public bool inputJump;
        public Vector3 movementVector;
        public bool IsGrounded => characterController.isGrounded;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            bool isMovingUpwards = verticalMovement > 0f;

            if (isMovingUpwards)
            {
                // Making sure the collision is near the top of the head
                float permittedDistance = characterController.radius / 2f;
                float topPositionY = transform.position.y + characterController.height;
                float distance = Mathf.Abs(hit.point.y - topPositionY);

                if (distance <= permittedDistance)
                {
                    // Stopping any upwards movement
                    // and having the player fall back down

                    isJumping = false;
                    gravityContributionMultiplier = 1f;

                    verticalMovement = 0f;
                }
            }
        }

        //---- COMMANDS ISSUED BY OTHER SCRIPTS ----

        public void Move(Vector3 movement)
        {
            inputVector = movement;
        }

        public void Jump()
        {
            inputJump = true;
        }

        public void CancelJump()
        {
            inputJump = false;
        }
    }
}