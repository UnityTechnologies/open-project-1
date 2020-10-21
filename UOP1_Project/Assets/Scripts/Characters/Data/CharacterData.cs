using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterData", menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject
{
	[Header("Movement")]

	[Tooltip("Horizontal XZ plane speed multiplier")] public float speed = 8f;
	[Tooltip("Smoothing for rotating the character to their movement direction")] public float turnSmoothTime = 0.2f;
	[Tooltip("Adjust the friction of the slope")] public float slideFriction = 0.3f;

	[Header("Jump")]

	[Tooltip("Number of jumps available for the player")] public int numberOfJumps = 1;
	[Tooltip("General multiplier for gravity (affects jump and freefall)")] public float gravityMultiplier = 5f;
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")] public float initialJumpForce = 10f;
	[Tooltip("How long can the player hold the jump button")] public float jumpInputDuration = .4f;
	[Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")] public float gravityComebackMultiplier = 15f;
	[Tooltip("The maximum speed reached when falling (in units/frame)")] public float maxFallSpeed = 50f;
	[Tooltip("Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")] public float gravityDivider = .6f;
	[Tooltip("Starting vertical movement when falling from a platform")] public float fallingVerticalMovement = -5f;

}
