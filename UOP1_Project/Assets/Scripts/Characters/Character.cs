using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    [Tooltip("Vertical falling direction reference object")] public Transform fallDirection;
    [Tooltip("Horizontal XZ plane speed multiplier")] public float speed = 8f;
    [Tooltip("Smoothing for rotating the character to their movement direction")] public float turnSmoothTime = 0.2f;
    [Tooltip("General multiplier for gravity (affects jump and freefall)")] public float gravityMultiplier = 5f;
    [Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")] public float initialJumpForce = 10f;
    [Tooltip("How long can the player hold the jump button")] public float jumpInputDuration = .4f;
    [Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")] public float gravityComebackMultiplier = 15f;
    [Tooltip("The maximum speed reached when falling (in units/frame)")] public float maxFallSpeed = 50f;
    [Tooltip("Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")] public float gravityDivider = .6f;

    private float gravityContributionMultiplier = 0f; //The factor which determines how much gravity is affecting verticalMovement
    private bool isJumping = false; //If true, a jump is in effect and the player is holding the jump button
    private float jumpBeginTime = -Mathf.Infinity; //Time of the last jump
    private float turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
    private float verticalMovement = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
    private float slopeAngle = 0f; //Represents the floor angle under the player. Used to check if it is a walkable slope based on CahracterController.SlopeLimit
    private float forwardAngle = 0f; //Represents the angle of character's forward in relation to the current slope
    private float forwardMultiplier = 0f;//Speed multiplier used when player is walking down a slope smaller than characterController.SlopeLimit
    private float fallMultiplier; //Falling speed multiplier used when player is sliding down a slope biggger than characterController.SlopeLimit
    private Vector3 collisionPoint; //Where exactly our character is colliding with the floor, used to fix an issue where he raycasts nothing while on a slope
    private Ray groundRay = new Ray(); //Ray used to check the ground below player
    private RaycastHit groundHit; //Represent the floor hit right below collisioPoint
    private Vector3 inputVector; //Initial input horizontal movement (y == 0f)
    private Vector3 movementVector; //Final movement vector

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Checks if character is in a slope to verify if he can walk/jump and if it'll affect it movement
        GroundDirection();

        //Raises the multiplier to how much gravity will affect vertical movement when in mid-air
        //This is 0f at the beginning of a jump and will raise to maximum 1f
        if (!characterController.isGrounded)
        {
            gravityContributionMultiplier += Time.deltaTime * gravityComebackMultiplier;
        }

        //Reduce the influence of the gravity while holding the Jump button
        if (isJumping)
        {
            //The player can only hold the Jump button for so long
            if (Time.time >= jumpBeginTime + jumpInputDuration)
            {
                isJumping = false;
                gravityContributionMultiplier = 1f; //Gravity influence is reset to full effect
            }
            else
            {
                gravityContributionMultiplier *= gravityDivider; //Reduce the gravity effect
            }
        }

        //Calculate the final verticalMovement
        if (!characterController.isGrounded || slopeAngle > characterController.slopeLimit)
        {
            if(slopeAngle > characterController.slopeLimit)
            {
                //blocks player movement if he's sliding down a slope
                movementVector = Vector3.zero;
                verticalMovement = Mathf.Lerp(0, -maxFallSpeed, 0.06f) * fallMultiplier; //applies a falling multiplier based on slope angle
            }
            else
            {
                //Less control in mid-air, conserving momentum from previous frame
                movementVector = inputVector * speed;

                //The character is either jumping or in freefall, so gravity will add up
                gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);
                verticalMovement += Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityContributionMultiplier; //Add gravity contribution
                                                                                                                            //Note that even if it's added, the above value is negative due to Physics.gravity.y
            }

            //Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
            verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
        }
        else if (characterController.isGrounded && slopeAngle < characterController.slopeLimit)
        {
            //Full speed ground movement
            movementVector = inputVector * speed;

            //Resets the verticalMovement while on the ground,
            //so that regardless of whether the player landed from a high fall or not,
            //if they drop off a platform they will always start with the same verticalMovement.
            //-5f is a good value to make it so the player also sticks to uneven terrain/bumps without floating.
            if (!isJumping)
            {
                verticalMovement = -5f;
                gravityContributionMultiplier = 0f;
            }
        }
        else
        {
            movementVector = Vector3.zero;
        }

        //Apply the result and move the character in space
        movementVector += fallDirection.up* verticalMovement;
        characterController.Move(movementVector * Time.deltaTime);

        //Rotate to the movement direction
        movementVector.y = 0f;
        if (movementVector.sqrMagnitude >= .02f)
        {
            float targetRotation = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref turnSmoothSpeed,
                turnSmoothTime);
        }
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //updates where the collider is hiting;
        collisionPoint = hit.point;
        collisionPoint = (collisionPoint - transform.position);
        if (isJumping)
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

    private void GroundDirection()
    {
        //setting up groundRay
        groundRay.origin = transform.position + collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        //reseting values
        forwardMultiplier = 1;
        fallMultiplier = 1;
        fallDirection.rotation = Quaternion.Euler(0, 0, 0);
        slopeAngle = 0;
        forwardAngle = 0;

        if (Physics.Raycast(groundRay, out groundHit, 0.5f))
        {
            //getting how steep is the slope and where the player is facing compared to the slope
            slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
            forwardAngle = Vector3.Angle(transform.forward, groundHit.normal) - 90;

            //if player is going down the slope, calculate a speed multiplier based on how steep is the slope
            if (forwardAngle < 0 && slopeAngle <= characterController.slopeLimit)
            {
                forwardMultiplier = 1 / Mathf.Cos(forwardAngle * Mathf.Deg2Rad);
            }
            else if (slopeAngle > characterController.slopeLimit)
            {
                fallMultiplier = 1 / Mathf.Cos((90 - slopeAngle) * Mathf.Deg2Rad);
                //get the falling angle to slide
                Vector3 groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
                fallDirection.rotation = Quaternion.FromToRotation(transform.up, Vector3.Cross(groundCross, groundHit.normal));
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
        if (characterController.isGrounded && slopeAngle <= characterController.slopeLimit)
        {
            isJumping = true;
            jumpBeginTime = Time.time;
            verticalMovement = initialJumpForce; //This is the only place where verticalMovement is set to a positive value
            gravityContributionMultiplier = 0f;
        }
    }

    public void CancelJump()
    {
        isJumping = false; //This will stop the reduction to the gravity, which will then quickly pull down the character
    }
}