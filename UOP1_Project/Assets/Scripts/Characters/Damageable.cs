using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private HealthSO _currentHealthSO;

	[Header("Combat")]
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;

	[Header("Broadcasting On")]
	[SerializeField] private VoidEventChannelSO _updateHealthUI = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;

	[Header("Listening To")]
	[SerializeField] private IntEventChannelSO _restoreHealth = default; //Getting cured when eating food

	public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;

	//Flags that the StateMachine uses for Conditions to move between states
	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer; //used to apply the hit flash effect

	public event UnityAction OnDie;

	private void Awake()
	{
		//If the HealthSO hasn't been provided in the Inspector (as it's the case for the player),
		//we create a new SO unique to this instance of the component. This is typical for enemies.
		if (_currentHealthSO == null)
		{
			_currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
			_currentHealthSO.SetMaxHealth(_healthConfigSO.InitialHealth);
			_currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
		}

		if (_updateHealthUI != null)
			_updateHealthUI.RaiseEvent();
	}

	private void OnEnable()
	{
		if(_restoreHealth != null)
			_restoreHealth.OnEventRaised += Cure;
	}

	private void OnDisable()
	{
		if(_restoreHealth != null)
			_restoreHealth.OnEventRaised -= Cure;
	}

	public void ReceiveAnAttack(int damage)
	{
		if (IsDead)
			return;

		_currentHealthSO.InflictDamage(damage);

		if (_updateHealthUI != null)
			_updateHealthUI.RaiseEvent();

		GetHit = true;

		if (_currentHealthSO.CurrentHealth <= 0)
		{
			IsDead = true;

			if (OnDie != null)
				OnDie.Invoke();

			if (_deathEvent != null)
				_deathEvent.RaiseEvent();

			_currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
		}
	}

	public void Kill()
	{
		ReceiveAnAttack(_currentHealthSO.CurrentHealth);
	}

	/// <summary>
	/// Called by the StateMachine action ResetHealthSO. Used to revive the Rock critters.
	/// </summary>
	public void Revive()
	{
		_currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
		
		if (_updateHealthUI != null)
			_updateHealthUI.RaiseEvent();
			
		IsDead = false;
	}

	/// <summary>
	/// Used for cure events, like eating food. Triggered by an IntEventChannelSO.
	/// </summary>
	private void Cure(int healthToAdd)
	{
		if (IsDead)
			return;
			
		_currentHealthSO.RestoreHealth(healthToAdd);

		if (_updateHealthUI != null)
			_updateHealthUI.RaiseEvent();
	}
}
