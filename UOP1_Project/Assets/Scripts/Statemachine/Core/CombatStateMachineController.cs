#pragma warning disable CS0649

using UnityEngine;

namespace CombatStatemachine
{
    public class CombatStateMachineController : MonoBehaviour
    {
        #region Inspector Vars
        [SerializeField] private CombatState m_initialState;
        [SerializeField] private HandlerDataSource m_dataSource;
        #endregion

        #region Properties
        public MovementHandler MoveHandler { get; private set; }
        #endregion

        #region Fields
        private CombatState m_currentState;
        #endregion


        #region Unity API
        private void Awake()
        {
            ChangeState(m_currentState);
            m_currentState.OnStateAwake(this);
        }
        private void OnEnable()
        {
            InitializeHandlers();
            m_currentState.OnStateEnable(this);
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
            MoveHandler = new MovementHandler(GetComponent<Transform>(), GetComponent<CharacterController>(), m_dataSource.MoveHandlerData);
        }
        private void CleanupHandlers()
        {
            MoveHandler.CleanupHandler();
            MoveHandler = null;
        }
        #endregion
    }

}
