#pragma warning disable CS0649

using UnityEngine;


namespace CombatStatemachine
{
    [CreateAssetMenu(menuName =CSMUtility.StateMachineRoot+"CombatAnimation",fileName ="CombatAnimation")]
    public class CombatAnimation : ScriptableObject
    {
        [SerializeField] private AnimationClip m_clip;
        public AnimationClip Clip { get { return m_clip; } }
    }

}
