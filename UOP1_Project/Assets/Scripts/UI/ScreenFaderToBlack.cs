using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFaderToBlack : MonoBehaviour
{
	public bool isBlack;
	[SerializeField] private BoolEventChannelSO fadeScreenToBlackEvent;
	[SerializeField] private float fadeFromBlackTime = 1;
	[SerializeField] private float fadeToBlackTime = 0;
	private GameObject blackScreen;
	private Image blackScreenImage;
	private bool _toBlack, _waitingToBlack;
	private bool _fromBlack, _waitingFromBlack;
	private Color _color;
	private float _alpha;
	private void OnEnable()
	{
		fadeScreenToBlackEvent.OnEventRaised += FadeScreenToBlack;
	}
	private void OnDisable()
	{
		fadeScreenToBlackEvent.OnEventRaised -= FadeScreenToBlack;
	}
	private void Awake()
	{
		blackScreen = transform.gameObject;
		blackScreenImage = blackScreen.GetComponent<Image>();
		//On awake make 
		blackScreenImage.enabled = false;
		_color = blackScreenImage.color;
		_color.a = 0;
		blackScreenImage.color = _color;
		_alpha = _color.a;

	}
	public void FadeScreenToBlack(bool toBlack)
	{
		if (toBlack)
		{
			FadeToBlack();
		}
		else
		{
			FadeFromBlack();
		}
	}
	private void FadeToBlack()
	{
		if (_fromBlack || _waitingFromBlack)
		{
			_waitingToBlack = true;
			return;
		}
		blackScreenImage.enabled = true;
		_toBlack = true;
		_fromBlack = false;
	}
	private void FadeFromBlack()
	{
		if (_toBlack || _waitingToBlack)
		{
			_waitingFromBlack = true;
			return;
		}
		blackScreenImage.enabled = true;
		_toBlack = false;
		_fromBlack = true;
	}

	private void SetColor()
	{
		_alpha = math.clamp(_alpha, 0, 1);
		_color.a = _alpha;
		blackScreenImage.color = _color;
	}
	private void Update()
	{
		//(_waitingToBlack && !_fromBlack) means that do only if ToBlack is on queue and fromBlack completed
		if (_toBlack || (_waitingToBlack && !_fromBlack))
		{
			if (_alpha < 1)
			{
				_alpha += Time.deltaTime * 1 / fadeToBlackTime;
				SetColor();
			}
			else
			{
				isBlack = true;
				_toBlack = false;
				_waitingToBlack = false;
			
			}
		}
		//(_waitingFromBlack && !_toBlack) means that do only if fromBlack is on queue and toBlack completed
		else if (_fromBlack || (_waitingFromBlack && !_toBlack))
		{
			if (_alpha > 0)
			{
				_alpha -= Time.deltaTime * 1 / fadeFromBlackTime;
				SetColor();
			}
			else
			{
				isBlack = false;
				_fromBlack = false;
				_waitingFromBlack = false;
				
				//if disable blackScreen when waitingToBlack - alpha will go up but image will be not visible
				if (!_waitingToBlack)
					blackScreenImage.enabled = false;
			}

		}
	}
}
