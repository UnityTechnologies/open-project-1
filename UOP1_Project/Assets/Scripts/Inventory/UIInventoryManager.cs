using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{

	[SerializeField]
	private Inventory _currentInventory = default;

	[SerializeField]
	private InventoryItemFiller _itemPrefab = default;

	[SerializeField]
	private GameObject _contentParent = default;

	[SerializeField]
	private InspectorFiller _inspectorFiller = default;

	[SerializeField]
	private InventoryTypeTabsFiller _tabFiller = default;

	[SerializeField]
	private InventoryButtonFiller _buttonFiller = default;

	InventoryTabType _selectedTab = default;

	[SerializeField]
	List<InventoryTabType> _tabTypesList = new List<InventoryTabType>();

	private int selectedItemId = -1;

	[SerializeField]
	private List<InventoryItemFiller> _instanciatedItems = default;


	[SerializeField]
	private ItemEventChannelSO _cookRecipeEvent = default;
	[SerializeField]
	private ItemEventChannelSO _useItemEvent = default;
	[SerializeField]
	private ItemEventChannelSO _equipItemEvent = default;

	[SerializeField]
	private TabEventChannelSO _changeTabEvent = default;

	[SerializeField]
	private ItemEventChannelSO _selectItemEvent = default;

	[SerializeField]
	private VoidEventChannelSO _actionButtonClicked = default;

	[SerializeField]
	private VoidEventChannelSO _onInteractionEndedEvent = default;

	[SerializeField]
	private InputReader _inputReader = default; 

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		
			_actionButtonClicked.OnEventRaised += ActionButtonEventRaised;
		
			_changeTabEvent.OnEventRaised += ChangeTabEventRaised;
		
			_selectItemEvent.OnEventRaised += InspectItem;
		
			_onInteractionEndedEvent.OnEventRaised += InteractionEnded;
		
		_inputReader.menuSwitchTab += SwitchTab;
		
	}

	private void OnDisable()
	{
			_actionButtonClicked.OnEventRaised -= ActionButtonEventRaised;
		
			_changeTabEvent.OnEventRaised -= ChangeTabEventRaised;
		
			_selectItemEvent.OnEventRaised -= InspectItem;
		
	}

	public void SwitchTab(float orientation)
	{
	
		if(orientation!=0)
		{
			bool isLeft = orientation < 0;
			int initialIndex = _tabTypesList.FindIndex(o => o == _selectedTab);
			if (initialIndex != -1)
			{
				if (isLeft)
				{
					initialIndex--;
				}
				else
				{
					initialIndex++;
				}

				initialIndex= Mathf.Clamp(initialIndex, 0, _tabTypesList.Count-1); 
			}
			
			ChangeTabEventRaised(_tabTypesList[initialIndex]); 
		}



	}

	bool isNearPot = false;
	public void FillInventory(TabType _selectedTabType = TabType.none, bool _isNearPot = false)
	{
		isNearPot = _isNearPot;

		if ((_selectedTabType != TabType.none) && (_tabTypesList.Exists(o => o.TabType == _selectedTabType)))
		{
			_selectedTab = _tabTypesList.Find(o => o.TabType == _selectedTabType);
		}
		else
		{
			if (_tabTypesList != null)
			{
				if (_tabTypesList.Count > 0)
				{
					_selectedTab = _tabTypesList[0];
				}
			}

		}


		if (_selectedTab != null)
		{
			FillTypeTabs(_tabTypesList, _selectedTab);
			List<ItemStack> listItemsToShow = new List<ItemStack>();
			listItemsToShow = _currentInventory.Items.FindAll(o => o.Item.ItemType.TabType == _selectedTab);
			
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

		_tabFiller.FillTabs(typesList, selectedType, _changeTabEvent);


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
					InventoryItemFiller instantiatedPrefab = Instantiate(_itemPrefab, _contentParent.transform) as InventoryItemFiller;
					_instanciatedItems.Add(instantiatedPrefab);

				}
				//fill

				bool isSelected = selectedItemId == i;
				_instanciatedItems[i].SetItem(listItemsToShow[i], isSelected, _selectItemEvent);


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
		//hover First Element
		if (_instanciatedItems.Count > 0)
		{
			_instanciatedItems[0].SelectFirstElement();
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
				if (_currentInventory.Items.Count > _instanciatedItems.Count)
				{
					//instantiate 
					InventoryItemFiller instantiatedPrefab = Instantiate(_itemPrefab, _contentParent.transform) as InventoryItemFiller;
					_instanciatedItems.Add(instantiatedPrefab);


				}
				//find the last instantiated game object not used
				index = _currentInventory.Items.Count;


			}

			//set item
			bool isSelected = selectedItemId == index;
			_instanciatedItems[index].SetItem(itemToUpdate, isSelected, _selectItemEvent);



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
			_buttonFiller.gameObject.SetActive(true);
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook)
			{
				isInteractable = _currentInventory.hasIngredients(itemToInspect.IngredientsList) && isNearPot;

			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.doNothing)
			{
				isInteractable = false;
				_buttonFiller.gameObject.SetActive(false);
			}

			//set button
			_buttonFiller.FillInventoryButtons(itemToInspect.ItemType, isInteractable);


		}

	}

	void ShowItemInformation(Item item)
	{

		bool[] availabilityArray = _currentInventory.IngredientsAvailability(item.IngredientsList);

		_inspectorFiller.FillItemInspector(item, availabilityArray);


	}
	void HideItemInformation()
	{
		_buttonFiller.gameObject.SetActive(false);
		_inspectorFiller.HideItemInspector();

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
		if (_actionButtonClicked != null)
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

		_useItemEvent.OnEventRaised(itemToUse);
		//update inventory
		FillInventory();
	}


	void EquipItem(Item itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		_equipItemEvent.OnEventRaised(itemToUse);
	}

	void CookRecipe(Item recipeToCook)
	{

		//get item
		_cookRecipeEvent.OnEventRaised(recipeToCook);

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
