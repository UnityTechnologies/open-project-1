#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    /*
     * Just an example class just for demonstration purposes.
     * In a real-world application you want to structure and manage dependencies 
     * (like 'Gun' and 'PlayerInputExample') in a more organized way.
     */
    [DisallowMultipleComponent]
    public class Character : MonoBehaviour
    {
        public Gun CurrentGun;
        public PlayerInputExample Input;

        [SerializeField]
        private float _jumpForce;
        [SerializeField]
        private Transform _body;
        [SerializeField]
        private Transform _equipment;

        public bool IsGrounded { get { return _rigidBody.position.y < .01f && !_jumpRequested; } }

        private Rigidbody2D _rigidBody;
        private bool _jumpRequested;
        private int _frameSkip;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Jump()
        {
            if (_jumpRequested)
                return;

            _frameSkip = 3;
            _jumpRequested = true;
        }

        public void Duck()
        {
            _body.localScale = new Vector3(1f, .5f, 1f);
            _body.localPosition = new Vector3(0f, .5f, 0f);
            _equipment.localPosition = new Vector3(0f, -.8f, 0f);
        }

        public void Stand()
        {
            _body.localScale = new Vector3(1f, 1f, 1f);
            _body.localPosition = new Vector3(0f, 1f, 0f);
            _equipment.localPosition = new Vector3(0f, 0f, 0f);
        }
     
        private void FixedUpdate()
        {
            if (_jumpRequested)
            {
                _rigidBody.velocity = Vector2.up * _jumpForce;
            }

            _frameSkip--;

            if (_frameSkip == 0)
                _jumpRequested = false;
        }
    }
}