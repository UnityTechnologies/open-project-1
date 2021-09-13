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
	[SerializeField] private VoidEventChannelSO _updateHealthEvent = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;

	[Header("Listening To")]
	[SerializeField] private IntEventChannelSO _restoreHealth = default;

	public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;

	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;

	public event UnityAction OnDie;

	private void Awake()
	{
		//If the HealthSO hasn't been provided in the Inspector (as it's the case for the player),
		//we create a new SO unique to this instance of the component. This is typical for NPCs.
		if (_currentHealthSO == null)
			_currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();

		_currentHealthSO.SetMaxHealth(_healthConfigSO.InitialHealth);
		_currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);

		if (_updateHealthEvent != null)
			_updateHealthEvent.RaiseEvent();
	}

	private void OnEnable()
	{
		_restoreHealth.OnEventRaised += RestoreHealth;
	}

	private void OnDisable()
	{
		_restoreHealth.OnEventRaised -= RestoreHealth;
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
			_updateHealthEvent.RaiseEvent();

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

	/// <summary>
	/// Called by the StateMachine action ResetHealth. Used to revive the stone critter.
	/// </summary>
	public void ResetHealth()
	{
		_currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);

		if (_updateHealthEvent != null)
			_updateHealthEvent.RaiseEvent();
			
		IsDead = false;
	}

	private void RestoreHealth(int healthToAdd)
	{
		if (IsDead)
			return;
			
		_currentHealthSO.RestoreHealth(healthToAdd);

		if (_updateHealthEvent != null)
			_updateHealthEvent.RaiseEvent();
	}
}
