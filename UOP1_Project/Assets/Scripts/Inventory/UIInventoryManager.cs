using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{

	[SerializeField]
	private Inventory currentInventory = default;
	[SerializeField]
	private InventoryItemFiller itemPrefab = default;
	[SerializeField]
	private GameObject contentParent = default;

	[SerializeField]
	private InspectorFiller inspectorFiller = default;

	[SerializeField]
	private InventoryTypeTabsFiller tabFiller = default;

	[SerializeField]
	private InventoryButtonFiller buttonFiller = default;

	InventoryTabType selectedTab = default;
	[SerializeField]
	List<InventoryTabType> tabTypesList = new List<InventoryTabType>();

	private int selectedItemId = -1;

	[SerializeField]
	private List<InventoryItemFiller> _instanciatedItems = default;


	[SerializeField]
	private ItemEventChannelSO CookRecipeEvent = default;
	[SerializeField]
	private ItemEventChannelSO UseItemEvent = default;
	[SerializeField]
	private ItemEventChannelSO EquipItemEvent = default;

	[SerializeField]
	private TabEventChannelSO ChangeTabEvent = default;

	[SerializeField]
	private ItemEventChannelSO SelectItemEvent = default;

	[SerializeField]
	private VoidEventChannelSO ActionButtonClicked = default;

	[SerializeField]
	private VoidEventChannelSO OnInteractionEndedEvent = default;

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

		if (_instanciatedItems == null)
			_instanciatedItems = new List<InventoryItemFiller>();

		int maxCount = Mathf.Max(listItemsToShow.Count, _instanciatedItems.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listItemsToShow.Count)
			{
				if (i >= _instanciatedItems.Count)
				{
					//instantiate 
					InventoryItemFiller instantiatedPrefab = Instantiate(itemPrefab, contentParent.transform) as InventoryItemFiller;
					_instanciatedItems.Add(instantiatedPrefab);

				}
				//fill

				bool isSelected = selectedItemId == i;
				_instanciatedItems[i].SetItem(listItemsToShow[i], isSelected, SelectItemEvent);


			}
			else if (i < _instanciatedItems.Count)
			{
				//Desactive
				_instanciatedItems[i].SetInactiveItem();
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
		if (_instanciatedItems == null)
			_instanciatedItems = new List<InventoryItemFiller>();

		if (removeItem)
		{
			if (_instanciatedItems.Exists(o => o._currentItem == itemToUpdate))
			{

				int index = _instanciatedItems.FindIndex(o => o._currentItem == itemToUpdate);
				_instanciatedItems[index].SetInactiveItem();

			}

		}
		else
		{
			int index = 0;
			//if the item has already been created
			if (_instanciatedItems.Exists(o => o._currentItem == itemToUpdate))
			{

				index = _instanciatedItems.FindIndex(o => o._currentItem == itemToUpdate);


			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (currentInventory.Items.Count > _instanciatedItems.Count)
				{
					//instantiate 
					InventoryItemFiller instantiatedPrefab = Instantiate(itemPrefab, contentParent.transform) as InventoryItemFiller;
					_instanciatedItems.Add(instantiatedPrefab);


				}
				//find the last instantiated game object not used
				index = currentInventory.Items.Count;


			}

			//set item
			bool isSelected = selectedItemId == index;
			_instanciatedItems[index].SetItem(itemToUpdate, isSelected, SelectItemEvent);



		}

	}



	public void InspectItem(Item itemToInspect)
	{
		if (_instanciatedItems.Exists(o => o._currentItem.Item == itemToInspect))
		{
			int itemIndex = _instanciatedItems.FindIndex(o => o._currentItem.Item == itemToInspect);


			//unselect selected Item
			if (selectedItemId >= 0 && selectedItemId != itemIndex)
				UnselectItem(selectedItemId);

			//change Selected ID 
			selectedItemId = itemIndex;

			//show Information
			ShowItemInformation(itemToInspect);

			//check if interactable
			bool isInteractable = true;
			buttonFiller.gameObject.SetActive(true);
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook)
			{
				isInteractable = currentInventory.hasIngredients(itemToInspect.IngredientsList) && isNearPot;

			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.doNothing)
			{
				isInteractable = false;
				buttonFiller.gameObject.SetActive(false);
			}

			//set button
			buttonFiller.FillInventoryButtons(itemToInspect.ItemType, isInteractable);


		}

	}

	void ShowItemInformation(Item item)
	{

		bool[] availabilityArray = currentInventory.IngredientsAvailability(item.IngredientsList);

		inspectorFiller.FillItemInspector(item, availabilityArray);


	}
	void HideItemInformation()
	{
		buttonFiller.gameObject.SetActive(false);
		inspectorFiller.HideItemInspector();

	}


	void UnselectItem(int itemIndex)
	{

		if (_instanciatedItems.Count > itemIndex)
		{
			_instanciatedItems[itemIndex].UnselectItem();

		}
	}

	void ActionButtonEventRaised()
	{
		if (ActionButtonClicked != null)
		{
			//find the selected Item
			if (_instanciatedItems.Count > selectedItemId && selectedItemId > -1)
			{
				//find the item 
				Item itemToActOn = new Item();
				itemToActOn = _instanciatedItems[selectedItemId]._currentItem.Item;

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
