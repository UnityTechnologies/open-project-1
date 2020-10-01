using UnityEngine;

namespace Inventory
{
    public abstract class ItemEffect : ScriptableObject
    {
        [Tooltip("The in-game name of the effect.")]
        [SerializeField]
        private string _name = string.Empty;

        [Tooltip("The description of the effect.")]
        [TextArea]
        [SerializeField]
        private string _description = string.Empty;

        public abstract void ApplyEffect(GameObject target);
    }
}