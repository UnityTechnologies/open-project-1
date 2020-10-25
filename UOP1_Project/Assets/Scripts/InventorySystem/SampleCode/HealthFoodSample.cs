using UnityEngine;

namespace InventorySystem.Sample
{
    public class HealthFoodSample : MonoBehaviour, IConsumableItem
    {
        public float healPercentage = 0.1f;
        public void ConsumeBy(GameObject source)
        {
            if (ReferenceEquals(source, null)) 
                return;
            source.SendMessage("Heal", healPercentage);
        }
    }
}