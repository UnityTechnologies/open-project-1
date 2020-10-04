#pragma warning disable CS0649

using System.Security.Policy;
using UnityEngine;

namespace CombatStatemachine
{
    public class CombatStateMachineController : MonoBehaviour
    {
        #region Inspector Vars
        [SerializeField] private Transform m_gameplayCamera;
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private CombatState m_initialState;
        [SerializeField] private HandlerDataSource m_dataSource;

        #endregion

        #region Properties
        public MovementHandler HandlerMovement { get; private set; }
        public InputHandler HandlerInput { get; private set; }
        public AnimationHandler HandlerAnimation { get; private set; }
        #endregion

        #region Fields
        private CombatState m_currentState;
        #endregion


        #region Unity API
       
        private void OnEnable()
        {
            InitializeHandlers();
        }
        private void Start()
        {
            ChangeState(m_initialState);
        }
        private void Update()
        {
            m_currentState.OnStateUpdate(this);
        }
        private void OnDisable()
        {
           
            m_currentState.OnStateDisable(this);

            CleanupHandlers();

        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
        }
        #endregion


        #region Public API
        public void ChangeState(CombatState _nextState)
        {
            if (m_currentState != null)
                m_currentState.OnStateExit(this);

            m_currentState = _nextState;

            m_currentState.OnStateEnter(this);
        }
        #endregion

        #region Utility
        private void InitializeHandlers()
        {
            HandlerMovement = new MovementHandler(m_gameplayCamera,GetComponent<Transform>(), GetComponent<CharacterController>(), m_dataSource.MoveHandlerData);

            HandlerInput = new InputHandler(m_dataSource.InHandlerData,m_inputReader);

            HandlerAnimation = new AnimationHandler(GetComponent<Animator>(), m_dataSource.AnimHandlerData);
        }
        private void CleanupHandlers()
        {
            HandlerMovement.CleanupHandler();
            HandlerMovement = null;

            HandlerInput.CleanupHandler();
            HandlerInput = null;

            HandlerAnimation.CleanupHandler();
            HandlerAnimation = null;
        }
        #endregion
    }

}
