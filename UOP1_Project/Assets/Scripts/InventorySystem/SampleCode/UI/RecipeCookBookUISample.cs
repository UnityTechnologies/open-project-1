using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Sample
{
    public class RecipeCookBookUISample : MonoBehaviour
    {
        public Canvas view;

        public int toolID;
        public List<int> combineItemList;
        
        public void OpenCraftView(int itemID)
        {
            ResetView();
            view.enabled = true;
            toolID = itemID;
        }

        private void ResetView()
        {
            combineItemList.Clear();
        }
    }
}