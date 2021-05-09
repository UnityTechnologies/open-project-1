using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class CollectableItem : MonoBehaviour
{

	[SerializeField] private Item _currentItem = default;
	[SerializeField] private GameObject _itemGO = default; 

	private void Start()
	{
		AnimateItem(); 
	}

	public Item GetItem()
	{

		return _currentItem;

	}
	public void SetItem(Item item)
	{
		_currentItem = item;

	}
	public void AnimateItem()
	{

		if(_itemGO!=null)
		{
			_itemGO.transform.DORotate(Vector3.one * 180, 5, RotateMode.Fast).SetLoops(-1,LoopType.Incremental); 

		}
	}

}
