using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInventory : MonoBehaviour
{
	public UnityAction Closed;

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private InventorySO _currentInventory = default;
	[SerializeField] private UIInventoryItem _itemPrefab = default;
	[SerializeField] private GameObject _contentParent = default;
	[SerializeField] private GameObject _errorPotMessage = default;
	[SerializeField] private UIInventoryInspector _inspectorPanel = default;
	[SerializeField] private List<InventoryTabSO> _tabTypesList = new List<InventoryTabSO>();
	[SerializeField] private List<UIInventoryItem> _availableItemSlots = default;

	[Header("Listening to")]
	[SerializeField] private UIInventoryTabs _tabsPanel = default;
	[SerializeField] private UIActionButton _actionButton = default;
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;

	[Header("Broadcasting on")]
	[SerializeField] private ItemEventChannelSO _useItemEvent = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
	[SerializeField] private ItemEventChannelSO _equipItemEvent = default;
	[SerializeField] private ItemEventChannelSO _cookRecipeEvent = default;

	private InventoryTabSO _selectedTab = default;
	private bool _isNearPot = false;
	private int selectedItemId = -1;

	private void OnEnable()
	{
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
		_onInteractionEndedEvent.OnEventRaised -= InteractionEnded;

		for (int i = 0; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ItemSelected -= InspectItem;
		}

		_inputReader.TabSwitched -= OnSwitchTab;
	}

	private void OnSwitchTab(float orientation)
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
			Debug.LogError("There's no selected tab");
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
				bool isSelected = selectedItemId == i;
				_availableItemSlots[i].SetItem(listItemsToShow[i], isSelected);

			}
			else if (i < _availableItemSlots.Count)
			{
				_availableItemSlots[i].SetInactiveItem();
			}

		}

		HideItemInformation();

		if (selectedItemId >= 0)
		{
			UnselectItem(selectedItemId);
			selectedItemId = -1;
		}
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
			if (_availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{

				int index = _availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
				_availableItemSlots[index].SetInactiveItem();
			}
		}
		else
		{
			int index = 0;

			//if the item has already been created
			if (_availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{
				index = _availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (_currentInventory.Items.Count > _availableItemSlots.Count)
				{
					UIInventoryItem instantiatedPrefab = Instantiate(_itemPrefab, _contentParent.transform) as UIInventoryItem;
					_availableItemSlots.Add(instantiatedPrefab);
				}

				//find the last instantiated game object not used
				index = _currentInventory.Items.Count;
			}

			bool isSelected = selectedItemId == index;
			_availableItemSlots[index].SetItem(itemToUpdate, isSelected);
		}
	}

	public void InspectItem(ItemSO itemToInspect)
	{
		if (_availableItemSlots.Exists(o => o.currentItem.Item == itemToInspect))
		{
			int itemIndex = _availableItemSlots.FindIndex(o => o.currentItem.Item == itemToInspect);

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
			_errorPotMessage.SetActive(false);
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.Cook)
			{
				isInteractable = _currentInventory.hasIngredients(itemToInspect.IngredientsList) && _isNearPot;
				_errorPotMessage.SetActive(!_isNearPot);
			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.DoNothing)
			{
				isInteractable = false;
				_actionButton.gameObject.SetActive(false);
			}

			//set button
			_actionButton.FillInventoryButton(itemToInspect.ItemType, isInteractable);
		}
	}

	void ShowItemInformation(ItemSO item)
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
		FillInventory(_selectedTab.TabType, _isNearPot);
	}

	void OnActionButtonClicked()
	{
		//find the selected Item
		if (_availableItemSlots.Count > selectedItemId
			&& selectedItemId > -1)
		{
			ItemSO itemToActOn = ScriptableObject.CreateInstance<ItemSO>();
			itemToActOn = _availableItemSlots[selectedItemId].currentItem.Item;

			//check the selected Item type
			//call action function depending on the itemType
			switch (itemToActOn.ItemType.ActionType)
			{
				case ItemInventoryActionType.Cook:
					CookRecipe(itemToActOn);
					break;
				case ItemInventoryActionType.Use:
					UseItem(itemToActOn);
					break;
				case ItemInventoryActionType.Equip:
					EquipItem(itemToActOn);
					break;
				default:

					break;
			}
		}
	}

	void UseItem(ItemSO itemToUse)
	{
		if (itemToUse.HealthResorationValue > 0)
		{ _restoreHealth.RaiseEvent(itemToUse.HealthResorationValue); }
		_useItemEvent.RaiseEvent(itemToUse);
		UpdateInventory();
	}

	void EquipItem(ItemSO itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		_equipItemEvent.RaiseEvent(itemToUse);
	}

	void CookRecipe(ItemSO recipeToCook)
	{
		_cookRecipeEvent.RaiseEvent(recipeToCook);

		//update inspector
		InspectItem(recipeToCook);

		//update inventory
		UpdateInventory();
	}

	void OnChangeTab(InventoryTabSO tabType)
	{
		FillInventory(tabType.TabType, _isNearPot);
	}

	public void CloseInventory()
	{
		Closed.Invoke();
	}
}