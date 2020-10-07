#pragma warning disable 0649
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
    [Serializable]
    public struct StateToStateTransition
    {
        public Object FromState;
        public Object ToState;
        public Object Condition;
        public bool ConditionResult;

        //Check if a entry has been left incompleted in the inspector
        public bool IsValid() => FromState != null && ToState != null && Condition != null;
    }

    [Serializable]
    public struct AnyStateToStateTransition
    {
        public Object ToState;
        public Object Condition;
        public bool ConditionResult;

        //Check if a entry has been left incompleted in the inspector
        public bool IsValid() => ToState != null && Condition != null;
    }

    //Stores State<T> and Condition<T> as Object (because inspector can't serialize generic classes) and converts
    //them into the proper types once when the game starts
    public abstract class TransitionTableBase : Initializable
    {
        [SerializeField]
        private Object _initialState;
        [SerializeField]
        private List<StateToStateTransition> _stateToStateEntries = new List<StateToStateTransition>();
        [SerializeField]
        private List<AnyStateToStateTransition> _anyStateToStateEntries = new List<AnyStateToStateTransition>();

        protected override void Initialize() => Initialize(_initialState, _stateToStateEntries, _anyStateToStateEntries);

        protected abstract void Initialize(Object initialState, List<StateToStateTransition> sts, List<AnyStateToStateTransition> asts);
        public abstract Type GetStateType();
        public abstract Type GetConditionType();
    }
}