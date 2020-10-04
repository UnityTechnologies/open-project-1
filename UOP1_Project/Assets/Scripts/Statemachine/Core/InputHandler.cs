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

        #region Fields
        private InputReader m_inputReader;
        #endregion

        #region public API
        public InputHandler(InputHandlerData _handlerData, InputReader _inputReader)
        {
            HandlerData = _handlerData;
            m_inputReader = _inputReader;
        }
        public void CleanupHandler()
        {
            m_inputReader = null;
        }
        public void SubscribeToEvents()
        {
            m_inputReader.jumpEvent += OnJumpInitiated;
            m_inputReader.jumpCanceledEvent += OnJumpCanceled;
            m_inputReader.moveEvent += OnMove;
        }
        public void UnsubscribeEvents()
        {
            m_inputReader.jumpEvent += OnJumpInitiated;
            m_inputReader.jumpCanceledEvent += OnJumpCanceled;
            m_inputReader.moveEvent += OnMove;
        }
        #endregion

        #region Utility


        #endregion

        #region InputEvents
        private void OnMove(Vector2 _movement)
        {
            Debug.Log("Movement " + _movement);
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
