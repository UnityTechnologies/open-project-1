using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AV.Logic
{
    public partial class StateMachine
    {
        // Fancy generic caching.
        // We are storing a dictionary per TComponent type thus avoiding the casting.
        // No more '(T)data' on every GetData or GetComponent call, just lookup by instance.
        private static class ComponentsCache<TComponent> where TComponent : Component
        {
            internal static readonly Dictionary<GameObject, TComponent> perInstance =
                new Dictionary<GameObject, TComponent>();
        }
        
        private static class SoCache<TScriptableObject> where TScriptableObject : ScriptableObject
        {
            internal static readonly Dictionary<GameObject, TScriptableObject> perInstance =
                new Dictionary<GameObject, TScriptableObject>();
        }
        
        private static class StatesCache<TStateData> where TStateData : IStateData
        {
            internal static readonly Dictionary<GameObject, TStateData> perInstance =
                new Dictionary<GameObject, TStateData>();
        }
        
        
#if UNITY_EDITOR
        // Mark dictionary as dirty to cleanup on scene unload or next Fast Play Mode.
        private static void MarkDictionaryDirty<TDataType>(IDictionary dictionary)
        {
            var dataType = typeof(TDataType);
            if (!dirtyDictionaries.TryGetValue(dataType, out _))
                dirtyDictionaries.Add(dataType, dictionary);
        }
#endif
        
        
#if UNITY_EDITOR
        private static int reloadCounter;
        private static readonly Dictionary<Type, IDictionary> dirtyDictionaries = new Dictionary<Type, IDictionary>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            SceneManager.sceneUnloaded += scene =>
            {
                // As we can have multiple scenes loaded, we need to remove items related to this scene only!
                // Note: not tested.
                foreach (var dictionary in dirtyDictionaries.Values)
                {
                    var instancesToRemove = new List<object>();
                    foreach (var value in dictionary.Values)
                    {
                        if(value is GameObject instance)
                            if(instance.scene == scene)
                                instancesToRemove.Add(instance);
                    }
                    foreach(var instance in instancesToRemove)
                        dictionary.Remove(instance);
                }
            };

            // Fast Play Mode compatibility.
            // https://docs.unity3d.com/2019.3/Documentation/Manual/DomainReloading.html
            if (reloadCounter > 0)
            {
                foreach (var dictionary in dirtyDictionaries.Values)
                    dictionary.Clear();
            }
            reloadCounter++;
        }
#endif

        
        public void SetData<TScriptableObject, TStateData>(TScriptableObject sharedData, TStateData runtimeData) 
            where TScriptableObject : ScriptableObject
            where TStateData : IStateData
        {
            SetSharedData(sharedData);
            SetData(runtimeData);
        }
        
        
        #region Components
        /// <summary>
        /// References <see cref="Component"/> to state machine instance.
        /// </summary>
        public void SetComponent<TComponent>(TComponent component) where TComponent : Component
        {
            if (ComponentsCache<TComponent>.perInstance.ContainsKey(gameObject))
            {
                ComponentsCache<TComponent>.perInstance[gameObject] = component;
            }
            else
            {
#if UNITY_EDITOR
                MarkDictionaryDirty<TComponent>(ComponentsCache<TComponent>.perInstance);
#endif
                ComponentsCache<TComponent>.perInstance.Add(gameObject, component);
            }
        }
        
        public bool HasComponent<TComponent>() where TComponent : Component
        {
            if (ComponentsCache<TComponent>.perInstance.ContainsKey(gameObject))
                return true;
            return TryGetComponent<TComponent>(out _);
        }
        
        /// <summary>
        /// <see cref="Component.TryGetComponent{TComponent}"/> from current instance and cache it.<para></para>
        /// </summary>
        public new bool TryGetComponent<TComponent>(out TComponent component) where TComponent : Component
        {
            if (ComponentsCache<TComponent>.perInstance.TryGetValue(gameObject, out component))
                return component;

            if (gameObject.TryGetComponent(out component))
            {
                ComponentsCache<TComponent>.perInstance.Add(gameObject, component);
                return true;
            }
            
            // Cache component as 'unity-null' so we won't get repeated TryGetComponent calls.
            ComponentsCache<TComponent>.perInstance.Add(gameObject, component);
            return false;
        }
        
        /// <summary>
        /// <see cref="Component.GetComponent{TComponent}"/> from current instance and cache it.<para></para>
        /// </summary>
        public new TComponent GetComponent<TComponent>() where TComponent : Component
        {
            if (ComponentsCache<TComponent>.perInstance.TryGetValue(gameObject, out var component))
                return component;

            component = gameObject.GetComponent<TComponent>();
            ComponentsCache<TComponent>.perInstance.Add(gameObject, component);

            return component;
        }
        #endregion
        
        
        #region Shared Data
        public void SetSharedData<TScriptableObject>(TScriptableObject sharedData) where TScriptableObject : ScriptableObject
        {
            if (SoCache<TScriptableObject>.perInstance.ContainsKey(gameObject))
            {
                SoCache<TScriptableObject>.perInstance[gameObject] = sharedData;
            }
            else
            {
#if UNITY_EDITOR
                MarkDictionaryDirty<TScriptableObject>(SoCache<TScriptableObject>.perInstance);
#endif
                SoCache<TScriptableObject>.perInstance.Add(gameObject, sharedData);
            }
        }
        
        public bool HasSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            return SoCache<TScriptableObject>.perInstance.ContainsKey(gameObject);
        }
        
        public bool TryGetSharedData<TScriptableObject>(out TScriptableObject sharedData) where TScriptableObject : ScriptableObject
        {
            return SoCache<TScriptableObject>.perInstance.TryGetValue(gameObject, out sharedData);
        }
        
        internal TScriptableObject GetSharedData<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            if (!SoCache<TScriptableObject>.perInstance.TryGetValue(gameObject, out var sharedData))
            {
                Debug.LogError($"{typeof(TScriptableObject)} shared-data was not referenced to {gameObject}. Do this before initialization via AttachData(ScriptableObject, IStateData).");
                return null;
            }

            return sharedData;
        }
        #endregion
        
        
        #region State Data
        public void SetData<TStateData>(TStateData runtimeData) where TStateData : IStateData
        {
            if (StatesCache<TStateData>.perInstance.ContainsKey(gameObject))
            {
                StatesCache<TStateData>.perInstance[gameObject] = runtimeData;
            }
            else
            {
#if UNITY_EDITOR
                MarkDictionaryDirty<TStateData>(StatesCache<TStateData>.perInstance);
#endif
                StatesCache<TStateData>.perInstance.Add(gameObject, runtimeData);
            }
        }
        
        public bool HasData<TStateData>() where TStateData : IStateData
        {
            return StatesCache<TStateData>.perInstance.ContainsKey(gameObject);
        }
        
        public bool TryGetData<TStateData>(out TStateData data) where TStateData : IStateData
        {
            return StatesCache<TStateData>.perInstance.TryGetValue(gameObject, out data);
        }
        
        public TStateData GetData<TStateData>() where TStateData : IStateData
        {
            if (!StatesCache<TStateData>.perInstance.TryGetValue(gameObject, out var component))
            {
                Debug.LogError($"{typeof(TStateData)} data was not attached to {gameObject}. Do this before initialization via AttachData(IStateData).");
                return default;
            }

            return component;
        }
        #endregion
    }
}