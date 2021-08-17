using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.Events;

public class UICreditsRoller : MonoBehaviour
{
	[SerializeField, Tooltip("Set speed of a rolling effect")] private float _speedPreset = 100f;//normal rolling speed
	[FormerlySerializedAs("speed")]
	[SerializeField, Tooltip("This is actuall speed of rolling")] private float _speed = 100f;//actual speed of rolling
	[SerializeField] private bool _rollAgain = false;
	[Header("References")]
	[FormerlySerializedAs("inputReader")]
	[SerializeField] private InputReader _inputReader = default;
	[FormerlySerializedAs("textCredits")]
	[SerializeField] private RectTransform _textCredits = default;
	[FormerlySerializedAs("mask")]
	[SerializeField] private RectTransform _mask = default;

	private float expectedFinishingPoint;

	public UnityAction rollingEnded;
	// Start is called before the first frame update
	public void StartRolling()
	{
		_speed = _speedPreset;
		Invoke("offsetStart", 0.01f);//This offset is needed to get true informations about rectangle and his mask

	}
	private void OnEnable()
	{
		_inputReader.moveEvent += OnMove;

	}
	private void OnDisable()
	{
		_inputReader.moveEvent -= OnMove;
	}
	// Update is called once per frame
	void Update()
	{

		//This make rolling effect
		if (_textCredits.anchoredPosition.y < expectedFinishingPoint)
		{
			_textCredits.anchoredPosition = new Vector2(_textCredits.anchoredPosition.x, _textCredits.anchoredPosition.y + _speed * Time.deltaTime);
		}
		else if (expectedFinishingPoint != 0)//this happend when rolling reach to end
		{
			RollingEnd();
		}
	}
	private void offsetStart()
	{
		_inputReader.EnableGameplayInput();
		expectedFinishingPoint = (_textCredits.rect.height + _mask.rect.height) / 2;

		_textCredits.anchoredPosition = new Vector2(_textCredits.anchoredPosition.x, -((_textCredits.rect.height + _mask.rect.height) / 2));

	}
	private void OnMove(Vector2 direction)
	{
		if (direction.y == 0f)//no horizontal movment
		{
			_speed = _speedPreset;
		}
		else if (direction.y > 0f)//upward movment
		{
			_speed = _speed * 2;
		}
		else//downward movment
		{
			_speed = -_speedPreset;
		}
	}
	private void RollingEnd()
	{
		if (_rollAgain)
		{   //reset postion of an element
			_textCredits.anchoredPosition = new Vector2(_textCredits.anchoredPosition.x, -((_textCredits.rect.height + _mask.rect.height) / 2));
		}
		else
		{
			rollingEnded.Invoke();
		}
	}

}
