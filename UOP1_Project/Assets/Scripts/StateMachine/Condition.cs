using UnityEngine;

namespace KarimCastagnini.PluggableFSM
{
    public abstract class Condition<T> : ScriptableObject
    {
        //implement this method to perform operations on dataModel
        public abstract bool IsMet(T dataModel);
    }
}