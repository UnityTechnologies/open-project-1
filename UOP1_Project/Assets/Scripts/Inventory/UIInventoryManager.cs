using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
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

	InventoryTabType selectedTab;
	[SerializeField]
	List<InventoryTabType> tabTypesList = new List<InventoryTabType>();

	private int selectedItemId = -1;

	private List<InventoryItemFiller> instantiatedGameObjects;


	public ItemEventChannelSo CookRecipeEvent;
	public ItemEventChannelSo UseItemEvent;
	public ItemEventChannelSo EquipItemEvent;

	public TabEventChannelSo ChangeTabEvent;

	public ItemEventChannelSo SelectItemEvent;

	public VoidEventChannelSO ActionButtonClicked;

	public VoidEventChannelSO OnInteractionEndedEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (ActionButtonClicked != null)
		{
			ActionButtonClicked.OnEventRaised += ActionButtonEventRaised;
		}
		if (ChangeTabEvent != null)
		{
			ChangeTabEvent.OnEventRaised += ChangeTabEventRaised;
		}
		if (SelectItemEvent != null)
		{
			SelectItemEvent.OnEventRaised += InspectItem;
		}
		if (OnInteractionEndedEvent != null)
		{
			OnInteractionEndedEvent.OnEventRaised += InteractionEnded;
		}
	}

	private void OnDisable()
	{
		if (ActionButtonClicked != null)
		{
			ActionButtonClicked.OnEventRaised -= ActionButtonEventRaised;
		}
		if (ChangeTabEvent != null)
		{
			ChangeTabEvent.OnEventRaised -= ChangeTabEventRaised;
		}
		if (SelectItemEvent != null)
		{
			SelectItemEvent.OnEventRaised -= InspectItem;
		}
	}



	bool isNearPot = false;
	public void FillInventory(TabType _selectedTabType = TabType.none, bool _isNearPot = false)
	{
		isNearPot = _isNearPot;

		if ((_selectedTabType != TabType.none) && (tabTypesList.Exists(o => o.TabType == _selectedTabType)))
		{
			selectedTab = tabTypesList.Find(o => o.TabType == _selectedTabType);
		}
		else
		{
			if (tabTypesList != null)
			{
				if (tabTypesList.Count > 0)
				{
					selectedTab = tabTypesList[0];
				}
			}

		}


		if (selectedTab != null)
		{
			FillTypeTabs(tabTypesList, selectedTab);
			List<ItemStack> listItemsToShow = currentInventory.Items.FindAll(o => o.Item.ItemType.TabType == selectedTab);
			FillItems(listItemsToShow);
		}
		else
		{

			Debug.Log("There's no item tab type ");

		}
	}
	public void InteractionEnded()
	{
		isNearPot = false;
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
				isInteractable = currentInventory.hasIngredients(itemToInspect.IngredientsList) && isNearPot
					;

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
			if (instantiatedGameObjects.Count > selectedItemId && selectedItemId > -1)
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

		UseItemEvent.OnEventRaised(itemToUse);
		//update inventory
		FillInventory();
	}


	void EquipItem(Item itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		EquipItemEvent.OnEventRaised(itemToUse);
	}

	void CookRecipe(Item recipeToCook)
	{

		//get item
		CookRecipeEvent.OnEventRaised(recipeToCook);

		//update inspector
		InspectItem(recipeToCook);

		//update inventory
		FillInventory();


	}

	void ChangeTabEventRaised(InventoryTabType tabType)
	{

		FillInventory(tabType.TabType);

	}

}
