using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
    //Stores Action<T> as Object (because inspector can't serialize generic classes) and converts it
    //into the proper type when the game starts.
    public abstract class StateBase : Initializable
    {
        [SerializeField] private List<Object> _onEnter = new List<Object>();
        [SerializeField] private List<Object> _onExit = new List<Object>();
        [SerializeField] private List<Object> _onUpdate = new List<Object>();

        protected override void Initialize() => Initialize(_onEnter, _onExit, _onUpdate);
        protected abstract void Initialize(List<Object> onEnter, List<Object> onExit, List<Object> onUpdate);

        public abstract Type GetActionType();
    }
}