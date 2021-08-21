using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;


	[Header("Broadcasting On")]
	[SerializeField] private IntEventChannelSO _setHealthBar = default;
	[SerializeField] private IntEventChannelSO _inflictDamage = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
	public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;

	private int _currentHealth = default;

	public bool GetHit { get; set; }
	public bool IsDead { get; set; }

	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;

	public int CurrentHealth => _currentHealth;

	public UnityAction OnDie;

	private void Awake()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
		if (_setHealthBar != null)
			_setHealthBar.RaiseEvent(_currentHealth);

	}

	public void Kill()
	{
		ReceiveAnAttack(_currentHealth);
	}

	public void ReceiveAnAttack(int damage)
	{
		if (IsDead)
			return;
		if (_inflictDamage != null)
			_inflictDamage.RaiseEvent(damage);
		_currentHealth -= damage;
		GetHit = true;
		if (_currentHealth <= 0)
		{
			IsDead = true;
			if (OnDie != null)
				OnDie.Invoke();
		}
	}

	public void ResetHealth()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
		if (_setHealthBar != null)
			_setHealthBar.RaiseEvent(_currentHealth);
		IsDead = false;
	}
	public void restoreHealth(int healthToAdd)
	{
		if (IsDead)
			return;
		_currentHealth += healthToAdd;
		if (_restoreHealth != null)
			_restoreHealth.RaiseEvent(healthToAdd);
	}
}
