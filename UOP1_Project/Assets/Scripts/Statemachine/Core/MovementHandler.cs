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
        public Transform GameplayCameraTransform { get; private set; }

        public float gravityContributionMultiplier = 0; //The factor which determines how much gravity is affecting verticalMovement
        public bool isJumping = false; //If true, a jump is in effect and the player is holding the jump button
        public float jumpBeginTime = -Mathf.Infinity; //Time of the last jump
        public float turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
        public float verticalMovement = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
        public Vector3 inputVector;//Initial input horizontal movement (y == 0f)
        public Vector3 movementVector; //Final movement vector
        #endregion

        #region Fields
        
        #endregion

        #region Public API
        public MovementHandler(Transform _gameplayCam,Transform _trans,CharacterController _charController, MovementHandlerData _data)
        {
            GameplayCameraTransform = _gameplayCam;
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
            GameplayCameraTransform = null;
            TransformComponent = null;
            CharControll = null;
        }

        #endregion

    }

}
