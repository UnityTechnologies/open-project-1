using System.Collections.Generic;
using UnityEngine;

public class InventoryFiller : MonoBehaviour
{
	[SerializeField]
	private Inventory currentInventory;
	[SerializeField]
	private InventoryItemFiller itemPrefab;
	[SerializeField]
	private GameObject contentParent;

	[SerializeField]
	private InspectorFiller inspectorFiller;

	[SerializeField]
	private InventoryTypeTabsFiller tabFiller;

	[SerializeField]
	private InventoryButtonFiller buttonFiller;

	InventoryTabType selectedTabType;
	[SerializeField]
	List<InventoryTabType> tabTypesList = new List<InventoryTabType>();

	private int selectedItemId = -1;

	private List<InventoryItemFiller> instantiatedGameObjects;


	public ItemEvent CookRecipeEvent;
	public ItemEvent UseItemEvent;
	public ItemEvent EquipItemEvent;

	public TabTypeEvent ChangeTabEvent;

	public ItemEvent SelectItemEvent;

	public VoidGameEvent ActionButtonClicked;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (ActionButtonClicked != null)
		{
			ActionButtonClicked.eventRaised += ActionButtonEventRaised;
		}
		if (ChangeTabEvent != null)
		{
			ChangeTabEvent.eventRaised += ChangeTabEventRaised;
		}
		if (SelectItemEvent != null)
		{
			SelectItemEvent.eventRaised += InspectItem;
		}
	}

	private void OnDisable()
	{
		if (ActionButtonClicked != null)
		{
			ActionButtonClicked.eventRaised -= ActionButtonEventRaised;
		}
		if (ChangeTabEvent != null)
		{
			ChangeTabEvent.eventRaised -= ChangeTabEventRaised;
		}
		if (SelectItemEvent != null)
		{
			SelectItemEvent.eventRaised -= InspectItem;
		}
	}




	public void FillInventory(InventoryTabType _selectedTabType = null)
	{


		if (_selectedTabType != null)
		{
			selectedTabType = _selectedTabType;
		}
		else
		{
			if (tabTypesList != null)
			{
				if (tabTypesList.Count > 0)
				{
					selectedTabType = tabTypesList[0];
				}
			}

		}


		if (selectedTabType != null)
		{
			FillTypeTabs(tabTypesList, selectedTabType);
			Debug.Log("Is changing tab");
			List<ItemStack> listItemsToShow = currentInventory.Items.FindAll(o => o.Item.ItemType.TabType == selectedTabType);
			FillItems(listItemsToShow);
		}
		else
		{

			Debug.Log("There's no item tab type ");

		}
	}

	void FillTypeTabs(List<InventoryTabType> typesList, InventoryTabType selectedType)
	{

		tabFiller.FillTabs(typesList, selectedType, ChangeTabEvent);


	}
	void FillItems(List<ItemStack> listItemsToShow)
	{

		if (instantiatedGameObjects == null)
			instantiatedGameObjects = new List<InventoryItemFiller>();

		int maxCount = Mathf.Max(listItemsToShow.Count, instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listItemsToShow.Count)
			{
				if (i >= instantiatedGameObjects.Count)
				{
					//instantiate 
					InventoryItemFiller instantiatedPrefab = Instantiate(itemPrefab, contentParent.transform) as InventoryItemFiller;
					instantiatedGameObjects.Add(instantiatedPrefab);

				}
				//fill

				bool isSelected = selectedItemId == i;
				instantiatedGameObjects[i].SetItem(listItemsToShow[i], isSelected, SelectItemEvent);
				instantiatedGameObjects[i].gameObject.SetActive(true);

			}
			else if (i < instantiatedGameObjects.Count)
			{
				//Desactive
				instantiatedGameObjects[i].gameObject.SetActive(false);
			}

		}
		HideItemInformation();
		//unselect selected Item
		if (selectedItemId >= 0)
		{
			UnselectItem(selectedItemId);
			selectedItemId = -1;
		}
	}
	public void UpdateOnItemInInventory(ItemStack itemToUpdate, bool removeItem)
	{
		if (instantiatedGameObjects == null)
			instantiatedGameObjects = new List<InventoryItemFiller>();

		if (removeItem)
		{
			if (instantiatedGameObjects.Exists(o => o.currentItem == itemToUpdate))
			{

				int index = instantiatedGameObjects.FindIndex(o => o.currentItem == itemToUpdate);
				instantiatedGameObjects[index].gameObject.SetActive(false);

			}

		}
		else
		{
			int index = 0;
			//if the item has already been created
			if (instantiatedGameObjects.Exists(o => o.currentItem == itemToUpdate))
			{

				index = instantiatedGameObjects.FindIndex(o => o.currentItem == itemToUpdate);


			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (currentInventory.Items.Count > instantiatedGameObjects.Count)
				{
					//instantiate 
					InventoryItemFiller instantiatedPrefab = Instantiate(itemPrefab, contentParent.transform) as InventoryItemFiller;
					instantiatedGameObjects.Add(instantiatedPrefab);


				}
				//find the last instantiated game object not used
				index = currentInventory.Items.Count;


			}

			//set item
			bool isSelected = selectedItemId == index;
			instantiatedGameObjects[index].SetItem(itemToUpdate, isSelected, SelectItemEvent);

			instantiatedGameObjects[index].gameObject.SetActive(true);


		}

	}



	public void InspectItem(Item itemToInspect)
	{
		if (instantiatedGameObjects.Exists(o => o.currentItem.Item == itemToInspect))
		{
			int itemIndex = instantiatedGameObjects.FindIndex(o => o.currentItem.Item == itemToInspect);


			//unselect selected Item
			if (selectedItemId >= 0 && selectedItemId != itemIndex)
				UnselectItem(selectedItemId);

			//change Selected ID 
			selectedItemId = itemIndex;

			//show Information
			ShowItemInformation(itemToInspect);

			//check if interactable
			bool isInteractable = true;
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook)
			{
				isInteractable = currentInventory.hasIngredients(itemToInspect.IngredientsList);

			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.doNothing)
			{
				isInteractable = false;

			}

			//set button
			buttonFiller.FillInventoryButtons(itemToInspect.ItemType, isInteractable);

		}

	}

	void ShowItemInformation(Item item)
	{

		bool[] availabilityArray = currentInventory.IngredietsAvailability(item.IngredientsList);

		inspectorFiller.FillItemInspector(item, availabilityArray);


	}
	void HideItemInformation()
	{

		inspectorFiller.HideItemInspector();

	}


	void UnselectItem(int itemIndex)
	{

		if (instantiatedGameObjects.Count > itemIndex)
		{
			instantiatedGameObjects[itemIndex].UnselectItem();

		}
	}

	void ActionButtonEventRaised()
	{
		if (ActionButtonClicked != null)
		{
			//find the selected Item
			if (instantiatedGameObjects.Count > selectedItemId)
			{
				//find the item 
				Item itemToActOn = new Item();
				itemToActOn = instantiatedGameObjects[selectedItemId].currentItem.Item;

				//check the selected Item type
				//call action function depending on the itemType
				switch (itemToActOn.ItemType.ActionType)
				{

					case ItemInventoryActionType.cook:
						CookRecipe(itemToActOn);
						break;
					case ItemInventoryActionType.use:
						UseItem(itemToActOn);
						break;
					case ItemInventoryActionType.equip:
						EquipItem(itemToActOn);
						break;
					default:

						break;

				}
			}
		}
	}

	void UseItem(Item itemToUse)
	{
		Debug.Log("USE ITEM " + itemToUse.name);

		UseItemEvent.Raise(itemToUse);
		//update inventory
		FillInventory();
	}


	void EquipItem(Item itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		EquipItemEvent.Raise(itemToUse);
	}

	void CookRecipe(Item recipeToCook)
	{

		//get item
		CookRecipeEvent.Raise(recipeToCook);

		//update inspector
		InspectItem(recipeToCook);

		//update inventory
		FillInventory();


	}

	void ChangeTabEventRaised(InventoryTabType tabType)
	{

		FillInventory(tabType);

	}

}
