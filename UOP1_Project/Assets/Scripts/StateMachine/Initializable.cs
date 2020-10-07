using UnityEditor;
using UnityEngine;

namespace KarimCastagnini.PluggableFSM
{
    //A class to perform initialization only when in PlayMode or when in Built, need to find a more elegant way
    public abstract class Initializable : ScriptableObject
    {
        private void OnEnable()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || Application.isPlaying)
                Initialize();
        }

        protected abstract void Initialize();
    }
}