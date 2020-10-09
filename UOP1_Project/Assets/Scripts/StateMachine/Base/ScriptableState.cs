
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace AV.Logic
{
    public abstract class ScriptableState : ScriptableObject
    {
        [HideInInspector] 
        protected internal StateMachine machine;

        protected GameObject gameObject => machine.gameObject;
        protected Transform transform => machine.transform;


        #region Shared Data
        /// <inheritdoc cref="StateMachine.SetSharedData{TScriptableObject}"/>
        protected TScriptableObject SetSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            return machine.GetSharedData<TScriptableObject>();
        }
        
        /// <inheritdoc cref="StateMachine.HasSharedData{TScriptableObject}"/>
        protected bool HasSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            return machine.HasSharedData<TScriptableObject>();
        }
        
        /// <inheritdoc cref="StateMachine.TryGetSharedData{TScriptableObject}"/>
        protected bool TryGetSharedData<TScriptableObject>(out TScriptableObject sharedData) where TScriptableObject : ScriptableObject
        {
            return machine.TryGetSharedData<TScriptableObject>(out sharedData);
        }
        
        /// <inheritdoc cref="StateMachine.GetSharedData{TScriptableObject}"/>
        protected TScriptableObject GetSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            return machine.GetSharedData<TScriptableObject>();
        }
        #endregion
        
        
        #region Components
        /// <inheritdoc cref="StateMachine.SetComponent{TComponent}"/>
        protected void SetComponent<TComponent>(TComponent component) where TComponent : Component
        {
            machine.SetComponent(component);
        }
        
        /// <inheritdoc cref="StateMachine.HasComponent{TComponent}"/>
        protected bool HasComponent<TComponent>() where TComponent : Component
        {
            return machine.HasComponent<TComponent>();
        }
        
        /// <inheritdoc cref="StateMachine.TryGetComponent{TComponent}"/>
        protected bool TryGetComponent<TComponent>(out TComponent component) where TComponent : Component
        {
            return machine.TryGetComponent(out component);
        }
        
        /// <inheritdoc cref="StateMachine.GetComponent{TComponent}"/>
        protected TComponent GetComponent<TComponent>() where TComponent : Component
        {
            return machine.GetComponent<TComponent>();
        }
        #endregion
        
        
        #region Data
        /// <inheritdoc cref="StateMachine.SetData{TStateData}"/>
        protected void SetData<TStateData>(TStateData runtimeData) where TStateData : IStateData
        {
            machine.SetData(runtimeData);
        }
        
        /// <inheritdoc cref="StateMachine.HasData{TStateData}"/>
        protected bool HasData<TStateData>() where TStateData : IStateData
        {
            return machine.HasData<TStateData>();
        }
        
        /// <inheritdoc cref="StateMachine.TryGetData{TStateData}"/>
        protected bool TryGetData<TStateData>(out TStateData data) where TStateData : IStateData
        {
            return machine.TryGetData<TStateData>(out data);
        }
        
        /// <inheritdoc cref="StateMachine.GetData{TStateData}"/>
        protected TStateData GetData<TStateData>() where TStateData : IStateData
        {
            return machine.GetData<TStateData>();
        }
        #endregion
    }
}