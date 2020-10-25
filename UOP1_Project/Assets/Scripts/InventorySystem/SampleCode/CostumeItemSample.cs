using UnityEngine;

namespace InventorySystem.Sample
{
    public class CostumeItemSample : MonoBehaviour, IEquipableItem
    {
        public bool isEquip = false;
        public GameObject equipCharacter;
        
        public bool IsEquipped()
        {
            return isEquip;
        }

        public void OnEquip(GameObject target)
        {
            isEquip = true;
            equipCharacter = target;
            transform.SetParent(target.transform);
            transform.localPosition = new Vector3(0,2f,0f);
        }

        public void OnUnEquip(GameObject target)
        {
            isEquip = false;
            GameObject.Destroy(gameObject);
        }
    }
}