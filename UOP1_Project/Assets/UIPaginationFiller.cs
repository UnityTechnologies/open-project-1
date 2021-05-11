using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class UIPaginationFiller : MonoBehaviour
{
	[SerializeField] private Image _imagePaginationPrefab= default;
	[SerializeField] private Transform _parentPagination = default;

	[SerializeField] private Sprite _emptyPagination = default;
	[SerializeField] private Sprite _filledPagination = default;

	private List<Image> _instantiatedImages = default;
	private void Start()
	{
		_instantiatedImages = new List<Image>(); 
	}
	public void SetPagination(int paginationCount, int selectedPaginationIndex)
	{

		//instanciate pagination images from the prefab
		int maxCount = Mathf.Max(paginationCount, _instantiatedImages.Count);
		Debug.Log(maxCount); 
		if (maxCount > 0)
		{
			for (int i = 0; i < maxCount; i++)
			{
				if (i >= _instantiatedImages.Count)
				{
					Image instantiatedImage = Instantiate(_imagePaginationPrefab, _parentPagination);
					_instantiatedImages.Add(instantiatedImage); 
				}

				if (i < paginationCount)
				{
					_instantiatedImages[i].gameObject.SetActive(true);

				}
				else
				{
					_instantiatedImages[i].gameObject.SetActive(false);

				}
			}

			SetCurrentPagination(selectedPaginationIndex);
		} 
	}

	public void SetCurrentPagination(int selectedPaginationIndex)
	{
		if (_instantiatedImages.Count > selectedPaginationIndex)
			for (int i = 0; i < _instantiatedImages.Count; i++)
			{
				if (i == selectedPaginationIndex)
				{
					_instantiatedImages[i].sprite = _filledPagination;

				}
				else
				{
					_instantiatedImages[i].sprite = _emptyPagination;


				}
			}
		else
			Debug.LogError("Error in pagination number"); 

	}
}
