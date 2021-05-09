using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[SerializeField] private HealthConfigSO _healthConfigSO;
	[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
	[SerializeField] private Renderer _mainMeshRenderer;
	[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;
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
	}

	public void ReceiveAnAttack(int damage)
	{
		_currentHealth -= damage;
		GetHit = true;
		if (_currentHealth <= 0)
		{
			IsDead = true;
			if (OnDie != null)
				OnDie.Invoke();
		}
	}
}
