using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private HealthSO _currentHealthSO;
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;


	[Header("Broadcasting On")]
	[SerializeField] private IntEventChannelSO _setHealthBar = default;
	[SerializeField] private IntEventChannelSO _inflictDamage = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;
	public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;


	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;


	public UnityAction OnDie;

	private void Awake()
	{
		if (_currentHealthSO == null)
		{
			_currentHealthSO = new HealthSO();
			_currentHealthSO.MaxHealth = _healthConfigSO.MaxHealth;
			_currentHealthSO.CurrentHealth = _healthConfigSO.MaxHealth;
		}

		if (_setHealthBar != null)
			_setHealthBar.RaiseEvent(_currentHealthSO.CurrentHealth);

	}

	public void Kill()
	{
		ReceiveAnAttack(_currentHealthSO.CurrentHealth);
	}

	public void ReceiveAnAttack(int damage)
	{
		if (IsDead)
			return;
		if (_inflictDamage != null)
			_inflictDamage.RaiseEvent(damage);
		_currentHealthSO.CurrentHealth -= damage;

		GetHit = true;
		if (_currentHealthSO.CurrentHealth <= 0)
		{
			IsDead = true;
			if (OnDie != null)
				OnDie.Invoke();
			if (_deathEvent != null)
				_deathEvent.RaiseEvent();
		}
	}

	public void ResetHealth()
	{
		_currentHealthSO.CurrentHealth = _healthConfigSO.MaxHealth;
		if (_setHealthBar != null)
			_setHealthBar.RaiseEvent(_currentHealthSO.CurrentHealth);
		IsDead = false;
	}
	public void restoreHealth(int healthToAdd)
	{
		if (IsDead)
			return;
		_currentHealthSO.CurrentHealth += healthToAdd;
		if (_restoreHealth != null)
			_restoreHealth.RaiseEvent(healthToAdd);
	}
}
