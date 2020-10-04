using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using System.Security.Policy;

namespace CombatStatemachine
{
    public class AnimationHandler
    {
        #region Properties
        public Animator AnimatorComponent { get; private set; }
        public AnimationHandlerData Data { get; private set; }
        #endregion

        #region Public API
        public AnimationHandler(Animator _animComponent, AnimationHandlerData _data)
        {
            AnimatorComponent = _animComponent;
            Data = _data;
        }
        public void CleanupHandler()
        {
            AnimatorComponent = null;
        }
        public void PlayAnimationClip(AnimationClip _clip)
        {

        }
        #endregion

    }
}

