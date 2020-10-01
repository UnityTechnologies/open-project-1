using UnityEngine;
using System.Collections;
using System.Security.Policy;

namespace CombatStatemachine
{
    public class MovementHandler
    {

        #region Properties
        public MovementHandlerData HandlerData { get; private set; }
        public CharacterController CharControll { get; private set; }
        public Transform TransformComponent { get; private set; }
        public float gravityContributionMultiplier { get; set; } = 0; //The factor which determines how much gravity is affecting verticalMovement
        public bool isJumping { get; set; } = false; //If true, a jump is in effect and the player is holding the jump button
        public float jumpBeginTime { get; set; } = -Mathf.Infinity; //Time of the last jump
        public float turnSmoothSpeed { get; set; } //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
        public float verticalMovement { get; set; } = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
        public Vector3 inputVector { get; set; } //Initial input horizontal movement (y == 0f)
        public Vector3 movementVector { get; set; } //Final movement vector
        #endregion

        #region Fields
        
        #endregion

        #region Public API
        public MovementHandler(Transform _trans,CharacterController _charController, MovementHandlerData _data)
        {
            TransformComponent = _trans;
            CharControll = _charController;
            HandlerData = _data;
        }
        public void CleanupHandler()
        {
            Cleanup();
        }
        #endregion

        #region Utility
        private void Cleanup()
        {
            TransformComponent = null;
            CharControll = null;
        }

        #endregion

    }

}
