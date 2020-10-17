using UnityEngine;

public class HungerComponent : MonoBehaviour
{
	[SerializeField] private float _maxFullness = 100f;
	[SerializeField] private float _hungerPerSecond = 1f;
	[SerializeField] [Range(0.01f, 0.99f)] private float _isHungryThreshold = 0.4f;
	private bool _getHungry;
	private float _currentFullness;
	public bool IsHungry => _currentFullness <= _maxFullness * _isHungryThreshold;

	private void Awake() => _currentFullness = _maxFullness;

	private void Update()
	{
		if (_getHungry)
			_currentFullness = Mathf.Max(_currentFullness - _hungerPerSecond * Time.deltaTime, 0f);
	}

	public void Eat(float amount)
		=> _currentFullness = Mathf.Min(_currentFullness + amount, _maxFullness);

	public void ToggleHunger(bool value) => _getHungry = value;
}
