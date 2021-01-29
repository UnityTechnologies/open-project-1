using UnityEngine;
using UnityEngine.Localization;

public class AttackableEntity : NpcEntity
{
	[SerializeField]
	private HealthConfigSO _healthConfigSO;

	[SerializeField]
	private GetHitEffectConfigSO _getHitEffectSO;

	[SerializeField]
	private Renderer _mainMeshRenderer;

	private int _currentHealth = default;

	public bool getHit { get; set; }
	public bool isDead { get; set; }
	
	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;
	
	private void Awake()
	{
		_currentHealth = _healthConfigSO.MaxHealth;
	}

	private void ReceiveAnAttack(int damange)
	{
		_currentHealth -= damange;
		getHit = true;
		if (_currentHealth <= 0)
		{
			isDead = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// Avoid friendly fire!
		if (!other.tag.Equals(gameObject.tag))
		{
			Weapon weapon = other.GetComponent<Weapon>();
			if (!getHit && weapon != null && weapon.Enable)
			{
				ReceiveAnAttack(weapon.AttackStrength);
			}
		}
	}
}
