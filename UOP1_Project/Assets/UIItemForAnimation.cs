using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIItemForAnimation : MonoBehaviour
{
	[SerializeField] private LocalizeSpriteEvent _bgLocalizedImage = default;
	[SerializeField] private Image _itemPreviewImage = default;
	[SerializeField] private Image _bgImage = default;

	public event UnityAction AnimationEnded;

	public void SetItem(ItemSO item)
	{
		if (item.IsLocalized)
		{
			_bgLocalizedImage.enabled = true;
			_bgLocalizedImage.AssetReference = item.LocalizePreviewImage;
		}
		else
		{
			_bgLocalizedImage.enabled = false;
			_itemPreviewImage.sprite = item.PreviewImage;
		}
		_bgImage.color = item.ItemType.TypeColor;
	}

	public void OnAnimationEnded()
	{
		AnimationEnded.Invoke();
	}
}
