using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace CombatStatemachine
{
    public class AnimationHandler
    {
        #region Fields
        private Animator m_animComponent;
        private AnimationHandlerData m_data;
        private AnimationClip m_currentClip;
        private PlayableGraph m_playableGraph;
        private AnimationPlayableOutput m_playableOutput;
        private AnimationClipPlayable m_clipPlayable;
        #endregion

        #region Public API
        public AnimationHandler(Animator _animComponent, AnimationHandlerData _data)
        {
            InitializeAnimator(_animComponent, _data);
            InitializePlayableGraph();
        }
        public void CleanupHandler()
        {
            Cleanup();
        }
        public void PlayAnimationClip(AnimationClip _clip)
        {
            SetAnimationClip(_clip);
            PlayCurrentAnimation();
        }
        #endregion

        #region Utility
        private void InitializeAnimator(Animator _animComponent, AnimationHandlerData _data)
        {
            m_animComponent = _animComponent;
            m_data = _data;
        }
        private void InitializePlayableGraph()
        {
            m_playableGraph = PlayableGraph.Create();
            m_playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            m_playableOutput = AnimationPlayableOutput.Create(m_playableGraph, "CombatAnimation", m_animComponent);
        }
        private void SetAnimationClip(AnimationClip _clip)
        {
            m_currentClip = _clip;
            m_clipPlayable = AnimationClipPlayable.Create(m_playableGraph, _clip);
            m_playableOutput.SetSourcePlayable(m_clipPlayable);

        }
        void PlayCurrentAnimation()
        {
            m_playableGraph.Play();
        }
        public void PlayAnimationManually(double _time)
        {
            m_clipPlayable.SetTime(_time);
        }
        private void Cleanup()
        {
            m_playableGraph.Destroy();
            m_animComponent = null;
        }
        #endregion

    }
}

