#pragma warning disable CS0649

using UnityEngine;
using System.Collections;

namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.HandlerDataRoot+"HandlerDataSource",fileName ="HandlerDataSource")]
    public class HandlerDataSource : ScriptableObject
    {
        [SerializeField] private MovementHandlerData m_moveHandlerData;
        public MovementHandlerData MoveHandlerData { get { return m_moveHandlerData; } }

        [SerializeField] private InputHandlerData m_inputHandlerData;
        public InputHandlerData InHandlerData { get { return m_inputHandlerData; } }
        
    }

}
