
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


        // Used only for internal invoke
        internal abstract void BeginUpdate(StateMachine machine);


        protected TComponent GetAttachedComponent<TComponent>() where TComponent : Component
        {
            return machine.GetAttachedComponent<TComponent>();
        }
        
        protected TScriptableObject GetSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            return machine.GetSharedData<TScriptableObject>();
        }
        
        protected TComponent GetComponent<TComponent>() where TComponent : Component
        {
            return machine.GetComponent<TComponent>();
        }
        
        protected TStateData GetData<TStateData>() where TStateData : IStateData
        {
            return machine.GetData<TStateData>();
        }

        protected bool TryGetData<TStateData>(out TStateData data) where TStateData : IStateData
        {
            return machine.TryGetData<TStateData>(out data);
        }

        protected bool HasData<TStateData>() where TStateData : IStateData
        {
            return machine.HasData<TStateData>();
        }

        protected void SetData<TStateData>(TStateData runtimeData) where TStateData : IStateData
        {
            machine.SetData(runtimeData);
        }
    }
}