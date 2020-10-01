using UnityEngine;

namespace Inventory.Effects
{
    [CreateAssetMenu(fileName = "HealingEffect", menuName = "Inventory/Effects/Healing", order = 52)]
    public class HealingEffect : ItemEffect
    {
        [Tooltip("The healing effect of the item.")]
        [SerializeField]
        private int _value = 0;

        public int Value => _value;

        public override void ApplyEffect(GameObject target)
        {
            // TODO: once the health system is in place
            Debug.Log($"Healing {target.name} for {_value}");
        }
    }
}