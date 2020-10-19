using UnityEngine;

namespace PlayerStateMachine.Actions
{
    public class MovingStateAction : PlayerStateAction
    {
        [SerializeField] private float _speedMultiplier = 1f;
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _cm.movementVector = _cm.inputVector * _cm.speed * _speedMultiplier;
            
            //Apply the result and move the character in space
            _cc.Move(_cm.movementVector * deltaTime);

            //Rotate to the movement direction
            _cm.movementVector.y = 0f;
            if (_cm.movementVector.sqrMagnitude >= .02f)
            {
                float targetRotation = Mathf.Atan2(_cm.movementVector.x, _cm.movementVector.z) * Mathf.Rad2Deg;
                _cm.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetRotation,
                    ref _cm.turnSmoothSpeed,
                    _cm.turnSmoothTime);
            }
        }
    }
}