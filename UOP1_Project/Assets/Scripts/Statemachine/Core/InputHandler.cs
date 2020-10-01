using UnityEngine;
using System.Collections;
using System.Security.Policy;

namespace CombatStatemachine
{
    public class InputHandler
    {
        #region Properties
        public InputHandlerData HandlerData { get; private set; }
        public Vector2 RawMovementInput { get; set; }
        public bool HasJumpInput { get; set; }
        #endregion

        #region public API
        public InputHandler(InputHandlerData _handlerData, InputReader _inputReader)
        {
            HandlerData = _handlerData;

            SubscribeToEvents(_inputReader);
        }
        public void CleanupHandler(InputReader _inputReader)
        {
            UnsubscribeEvents(_inputReader);
        }
        #endregion

        #region Utility
        private void SubscribeToEvents(InputReader _inputReader)
        {
            _inputReader.jumpEvent += OnJumpInitiated;
            _inputReader.jumpCanceledEvent += OnJumpCanceled;
            _inputReader.moveEvent += OnMove;
        }
        private void UnsubscribeEvents(InputReader _inputReader)
        {
            _inputReader.jumpEvent += OnJumpInitiated;
            _inputReader.jumpCanceledEvent += OnJumpCanceled;
            _inputReader.moveEvent += OnMove;
        }

        #endregion

        #region InputEvents
        private void OnMove(Vector2 _movement)
        {
            RawMovementInput = _movement;
        }

        private void OnJumpInitiated()
        {
            HasJumpInput = true;
        }

        private void OnJumpCanceled()
        {
            HasJumpInput = false;
        }
        #endregion
    }

}
