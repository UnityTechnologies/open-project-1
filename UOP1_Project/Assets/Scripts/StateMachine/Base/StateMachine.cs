using System;
using System.Collections.Generic;
using UnityEngine;

namespace AV.Logic
{
    [AddComponentMenu("Logic/State Machine")]
    public class StateMachine : MonoBehaviour
    {
        // TODO: Support multiple states at once.
        // TODO: StateFlow class for storing and visualizing states relation in a nice editor.

        public StateNode currentState;
        [HideInInspector]
        internal bool[] previousDecisions;
        [HideInInspector]
        internal new Transform transform;

        // TODO: Runtime data inspector.
        private Dictionary<Type, Component> cachedComponents;
        private Dictionary<Type, Component> attachedComponents;
        private readonly Dictionary<Type, IStateData> attachedData = new Dictionary<Type, IStateData>();
        
        private static readonly Dictionary<Type, ScriptableObject> AllSharedData = new Dictionary<Type, ScriptableObject>();

        #if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnDomainReload()
        {
            AllSharedData.Clear();
        }
        #endif
        
        public void Initialize()
        {
            transform = base.transform;

            if (!currentState)
                return;

            InitializeTransitionsData(currentState);
            CheckStateTriggers(currentState, StateTrigger.OnStart);
        }

        private void InitializeTransitionsData(StateNode state)
        {
            previousDecisions = new bool[state.transitions.Length];
        }
        
        public void Run()
        {
            if (!currentState)
                return;

            currentState.machine = this;
            currentState.UpdateActions(this);
            currentState.CheckTransitions(this);
        }

        public void EnterState(StateNode nextState)
        {
            if (!nextState)
                return;
            
            CheckStateTriggers(currentState, StateTrigger.OnExit);
                
            currentState = nextState;

            InitializeTransitionsData(nextState);
            CheckStateTriggers(nextState, StateTrigger.OnEnter);
        }
        
        private void CheckStateTriggers(StateNode state, StateTrigger trigger)
        {
            foreach (var action in state.actions)
            {
                if (action.trigger != trigger || !action.logic)
                    continue;
                action.logic.BeginUpdate(this);
            }
        }

        
        // TODO: Getting data introduces unboxing to generic on every update. How can we avoid this?

        internal TScriptableObject GetSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            var type = typeof(TScriptableObject);
            
            if (!AllSharedData.TryGetValue(type, out var sharedData))
            {
                Debug.LogError($"{type} shared-data was not referenced to StateMachine. Do this before initialization via AttachData(ScriptableObject, IStateData).");
                return null;
            }

            return (TScriptableObject)sharedData;
        }
        
        /// <summary>
        /// GetComponent from game object and cache it inside state machine instance.<para></para>
        /// Note: we can't store components inside ScriptableObject as they would be shared across all instances.
        /// </summary>
        public new TComponent GetComponent<TComponent>() where TComponent : Component
        {
            var type = typeof(TComponent);
            
            if(cachedComponents == null)
                cachedComponents = new Dictionary<Type, Component>();
            
            if (!cachedComponents.TryGetValue(type, out var component))
            {
                component = base.GetComponent<TComponent>();
                cachedComponents.Add(type, component);
            }

            return (TComponent)component;
        }
        
        /// <summary>
        /// Returns component that was attached to state machine via <see cref="AttachComponent{TComponent}"/>.
        /// </summary>
        public TComponent GetAttachedComponent<TComponent>() where TComponent : Component
        {
            var type = typeof(TComponent);
            
            if(attachedComponents == null)
                attachedComponents = new Dictionary<Type, Component>();
            
            if (!attachedComponents.TryGetValue(type, out var component))
            {
                Debug.LogError($"{type} component was not attached to StateMachine. Use AttachComponent(TComponent) first.");
                return null;
            }

            return (TComponent)component;
        }

        public bool HasData<TStateData>() where TStateData : IStateData
        {
            return attachedData.ContainsKey(typeof(TStateData));
        }
        
        public bool TryGetData<TStateData>(out TStateData data) where TStateData : IStateData
        {
            var type = typeof(TStateData);
            var contains = attachedData.TryGetValue(type, out var stateData);
            data = contains ? (TStateData)stateData : default;
            return contains;
        }
        
        public TStateData GetData<TStateData>() where TStateData : IStateData
        {
            var type = typeof(TStateData);
            if (!attachedData.TryGetValue(type, out var component))
            {
                Debug.LogError($"{type} data was not attached to StateMachine. Do this before initialization via AttachData(IStateData).");
                return default;
            }

            return (TStateData)component;
        }

        public void SetData<TStateData>(TStateData runtimeData) where TStateData : IStateData
        {
            var type = typeof(TStateData);
            if (attachedData.ContainsKey(type))
            {
                attachedData[type] = runtimeData;
            }
            else
            {
                attachedData.Add(type, runtimeData);
            }
        }
        
        public void AttachComponent<TComponent>(TComponent component) where TComponent : Component
        {
            var type = typeof(TComponent);
            
            if(attachedComponents == null)
                attachedComponents = new Dictionary<Type, Component>();
            
            if(attachedComponents.ContainsKey(type))
                Debug.LogError($"{type} component is already attached to StateMachine.");
                
            attachedComponents.Add(type, component);
        }

        public void AttachData<TStateData>(TStateData runtimeData) where TStateData : IStateData
        {
            attachedData.Add(typeof(TStateData), runtimeData);
        }

        public void AttachSharedData<TScriptableObject>(TScriptableObject sharedData) where TScriptableObject : ScriptableObject
        {
            if(!AllSharedData.ContainsKey(typeof(TScriptableObject)))
                AllSharedData.Add(typeof(TScriptableObject), sharedData);
        }
        
        public void AttachData<TScriptableObject, TStateData>(TScriptableObject sharedData, TStateData runtimeData) 
            where TScriptableObject : ScriptableObject
            where TStateData : IStateData
        {
            AttachSharedData(sharedData);
            attachedData.Add(typeof(TStateData), runtimeData);
        }
    }
}