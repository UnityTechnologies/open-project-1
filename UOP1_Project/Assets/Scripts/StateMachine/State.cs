using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
    public abstract class State<T> : StateBase
    {
        //collection of actions to perform once every time the FSM transits INTO this state
        private List<Action<T>> _onEnterActions = new List<Action<T>>();
        //collection of actions to perform once every time the FSM transits OUT this state
        private List<Action<T>> _onExitActions = new List<Action<T>>();
        //collection of actions to perform while the FSM is in this state
        private List<Action<T>> _onUpdateActions = new List<Action<T>>();

        public void OnEnter(T dataModel) => DoActions(_onEnterActions, dataModel);
        public void OnExit(T dataModel) => DoActions(_onExitActions, dataModel);
        public void OnUpdate(T dataModel) => DoActions(_onUpdateActions, dataModel);

        private void DoActions(List<Action<T>> actions, T dataModel)
        {
            foreach (Action<T> action in actions)
                action.Act(dataModel);
        }
       
        #region Inspector utils

        private static Type _actionTypeCache;

        public override Type GetActionType()
        {
            if (_actionTypeCache == null)
                _actionTypeCache = typeof(Action<T>);

            return _actionTypeCache;
        }

        //called by StateBase only once when we enter PlayMode or in Built
        protected override void Initialize(List<Object> onEnter, List<Object> onExit, List<Object> onUpdate)
        {
            ConvertToActions(onEnter, _onEnterActions);
            ConvertToActions(onExit, _onExitActions);
            ConvertToActions(onUpdate, _onUpdateActions);
        }

        private void ConvertToActions(List<Object> source, List<Action<T>> dest)
        {
            foreach (Object o in source)
                if (o != null)
                    dest.Add((Action<T>)o);
        }
        #endregion
    }
}