using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UIInventory : MonoBehaviour
{

	[SerializeField]
	private Inventory _currentInventory = default;

	[SerializeField]
	private UIInventoryItem _itemPrefab = default;

	[SerializeField]
	private GameObject _contentParent = default;

	[FormerlySerializedAs("_inspectorFiller")]
	[SerializeField]
	private UIInventoryInspector _inspectorPanel = default;

	[FormerlySerializedAs("_tabFiller")]
	[SerializeField]
	private UIInventoryTabs _tabsPanel = default;

	[FormerlySerializedAs("_buttonFiller")]
	[SerializeField]
	private UIInventoryActionButton _actionButton = default;

	InventoryTabSO _selectedTab = default;

	[SerializeField]
	List<InventoryTabSO> _tabTypesList = new List<InventoryTabSO>();

	private int selectedItemId = -1;

	[FormerlySerializedAs("_instanciatedItems")]
	[SerializeField]
	private List<UIInventoryItem> _availableItemSlots = default;

	[SerializeField]
	private VoidEventChannelSO _onInteractionEndedEvent = default;

	[SerializeField]
	private ItemEventChannelSO _useItemEvent = default;
	[SerializeField]
	private ItemEventChannelSO _equipItemEvent = default;
	[SerializeField]
	private ItemEventChannelSO _cookRecipeEvent = default;

	[SerializeField]
	private InputReader _inputReader = default;

	public UnityAction Closed;
	bool _isNearPot = false;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors

		_actionButton.Clicked += OnActionButtonClicked;

		_tabsPanel.TabChanged += OnChangeTab;


		_onInteractionEndedEvent.OnEventRaised += InteractionEnded;


		for (int i = 0; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ItemSelected += InspectItem;
		}

		_inputReader.TabSwitched += OnSwitchTab;
	}

	private void OnDisable()
	{
		_actionButton.Clicked -= OnActionButtonClicked;

		_tabsPanel.TabChanged -= OnChangeTab;

		for (int i = 0; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ItemSelected -= InspectItem;
		}

		_inputReader.TabSwitched -= OnSwitchTab;
	}

	void OnSwitchTab(float orientation)
	{

		if (orientation != 0)
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

				initialIndex = Mathf.Clamp(initialIndex, 0, _tabTypesList.Count - 1);
			}

			OnChangeTab(_tabTypesList[initialIndex]);
		}



	}

	public void FillInventory(InventoryTabType _selectedTabType = InventoryTabType.CookingItem, bool isNearPot = false)
	{
		_isNearPot = isNearPot;

		if ((_tabTypesList.Exists(o => o.TabType == _selectedTabType)))
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
			SetTabs(_tabTypesList, _selectedTab);
			List<ItemStack> listItemsToShow = new List<ItemStack>();
			listItemsToShow = _currentInventory.Items.FindAll(o => o.Item.ItemType.TabType == _selectedTab);

			FillInvetoryItems(listItemsToShow);
		}
		else
		{

			Debug.LogError("There's no selected tab ");

		}
	}

	void InteractionEnded()
	{
		_isNearPot = false;
	}

	void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
	{

		_tabsPanel.SetTabs(typesList, selectedType);


	}
	void FillInvetoryItems(List<ItemStack> listItemsToShow)
	{

		if (_availableItemSlots == null)
			_availableItemSlots = new List<UIInventoryItem>();

		int maxCount = Mathf.Max(listItemsToShow.Count, _availableItemSlots.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listItemsToShow.Count)
			{

				//fill
				bool isSelected = selectedItemId == i;
				_availableItemSlots[i].SetItem(listItemsToShow[i], isSelected);

			}
			else if (i < _availableItemSlots.Count)
			{
				//Desactive
				_availableItemSlots[i].SetInactiveItem();
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
		if (_availableItemSlots.Count > 0)
		{
			_availableItemSlots[0].SelectFirstElement();
		}

	}

	void UpdateItemInInventory(ItemStack itemToUpdate, bool removeItem)
	{
		if (_availableItemSlots == null)
			_availableItemSlots = new List<UIInventoryItem>();

		if (removeItem)
		{
			if (_availableItemSlots.Exists(o => o._currentItem == itemToUpdate))
			{

				int index = _availableItemSlots.FindIndex(o => o._currentItem == itemToUpdate);
				_availableItemSlots[index].SetInactiveItem();

			}

		}
		else
		{
			int index = 0;
			//if the item has already been created
			if (_availableItemSlots.Exists(o => o._currentItem == itemToUpdate))
			{

				index = _availableItemSlots.FindIndex(o => o._currentItem == itemToUpdate);


			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (_currentInventory.Items.Count > _availableItemSlots.Count)
				{
					//instantiate 
					UIInventoryItem instantiatedPrefab = Instantiate(_itemPrefab, _contentParent.transform) as UIInventoryItem;
					_availableItemSlots.Add(instantiatedPrefab);


				}
				//find the last instantiated game object not used
				index = _currentInventory.Items.Count;


			}

			//set item
			bool isSelected = selectedItemId == index;
			_availableItemSlots[index].SetItem(itemToUpdate, isSelected);


		}

	}



	public void InspectItem(Item itemToInspect)
	{
		if (_availableItemSlots.Exists(o => o._currentItem.Item == itemToInspect))
		{
			int itemIndex = _availableItemSlots.FindIndex(o => o._currentItem.Item == itemToInspect);


			//unselect selected Item
			if (selectedItemId >= 0 && selectedItemId != itemIndex)
				UnselectItem(selectedItemId);

			//change Selected ID 
			selectedItemId = itemIndex;

			//show Information
			ShowItemInformation(itemToInspect);

			//check if interactable
			bool isInteractable = true;
			_actionButton.gameObject.SetActive(true);
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook)
			{
				isInteractable = _currentInventory.hasIngredients(itemToInspect.IngredientsList) && _isNearPot;

			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.doNothing)
			{
				isInteractable = false;
				_actionButton.gameObject.SetActive(false);
			}

			//set button
			_actionButton.FillInventoryButton(itemToInspect.ItemType, isInteractable);


		}

	}

	void ShowItemInformation(Item item)
	{

		bool[] availabilityArray = _currentInventory.IngredientsAvailability(item.IngredientsList);

		_inspectorPanel.FillInspector(item, availabilityArray);
		_inspectorPanel.gameObject.SetActive(true);

	}
	void HideItemInformation()
	{
		_actionButton.gameObject.SetActive(false);
		_inspectorPanel.gameObject.SetActive(false);

	}


	void UnselectItem(int itemIndex)
	{

		if (_availableItemSlots.Count > itemIndex)
		{
			_availableItemSlots[itemIndex].UnselectItem();

		}
	}
	void UpdateInventory()
	{
		FillInventory(_selectedTab.TabType);
	}

	void OnActionButtonClicked()
	{

		//find the selected Item
		if (_availableItemSlots.Count > selectedItemId && selectedItemId > -1)
		{
			//find the item 
			Item itemToActOn = new Item();
			itemToActOn = _availableItemSlots[selectedItemId]._currentItem.Item;

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
	void UseItem(Item itemToUse)
	{
		Debug.Log("USE ITEM " + itemToUse.name);

		_useItemEvent.OnEventRaised(itemToUse);
		//update inventory
		UpdateInventory();
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
		UpdateInventory();


	}

	void OnChangeTab(InventoryTabSO tabType)
	{

		FillInventory(tabType.TabType);

	}
	public void CloseInventory()
	{
		Closed.Invoke();
	}

}
