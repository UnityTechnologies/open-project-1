using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonPrompt : MonoBehaviour
{

	[SerializeField] private Image _interactionKeyBG = default;
	[SerializeField] private TextMeshProUGUI _interactionKeyText = default;
	[SerializeField] private Sprite _controllerSprite = default;
	[SerializeField] private Sprite _keyboardSprite = default;
	[SerializeField] private string _interactionKeyboardCode = default;
	[SerializeField] private string _interactionJoystickKeyCode = default;

	public void SetButtonPrompt(bool isKeyboard)
	{
		if (!isKeyboard)
		{
			_interactionKeyBG.sprite = _controllerSprite;
			_interactionKeyText.text = _interactionJoystickKeyCode;
		}
		else
		{
			_interactionKeyBG.sprite = _keyboardSprite;
			_interactionKeyText.text = _interactionKeyboardCode;
		}
	}
}
