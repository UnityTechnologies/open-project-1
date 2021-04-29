using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonPromptSetter : MonoBehaviour
{

	[SerializeField] Image _interactionKeyBG = default;

	[SerializeField] TextMeshProUGUI _interactionKeyText = default;

	[SerializeField] Sprite _controllerSprite = default;
	[SerializeField] Sprite _keyboardSprite = default;

	[SerializeField] string _interactionKeyboardCode = default;
	[SerializeField] string _interactionJoystickKeyCode = default;

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
