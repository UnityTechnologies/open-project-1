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
	[SerializeField] private VoidEventChannelSO _updateHealthEvent = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;
	[Header("Listening To")]
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
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
			_currentHealthSO.SetMaxHealth(_healthConfigSO.MaxHealth);
			_currentHealthSO.SetCurrentHealth(_healthConfigSO.MaxHealth);
		}
		if (_updateHealthEvent != null)
		{
			_updateHealthEvent.RaiseEvent();

		}

	}
	private void OnEnable()
	{
		if (_restoreHealth != null)
		{
			_restoreHealth.OnEventRaised += restoreHealth;

		}
	}
	private void OnDisable()
	{
		if (_restoreHealth != null)
		{
			_restoreHealth.OnEventRaised -= restoreHealth;

		}
	}
	public void Kill()
	{
		ReceiveAnAttack(_currentHealthSO.CurrentHealth);
	}

	public void ReceiveAnAttack(int damage)
	{
		if (IsDead)
			return;

		_currentHealthSO.InflictDamage(damage);
		if (_updateHealthEvent != null)
		{
			_updateHealthEvent.RaiseEvent();

		}
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
		_currentHealthSO.SetCurrentHealth(_healthConfigSO.MaxHealth);
		if (_updateHealthEvent != null)
		{
			_updateHealthEvent.RaiseEvent();

		}
		IsDead = false;
	}
	public void restoreHealth(int healthToAdd)
	{
		if (IsDead)
			return;
		_currentHealthSO.RestoreHealth(healthToAdd);
		if (_updateHealthEvent != null)
		{
			_updateHealthEvent.RaiseEvent();

		}
	}
}
