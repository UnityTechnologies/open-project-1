using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "EntityConfig/Health Config")]
public class HealthConfigSO : ScriptableObject
{
	[SerializeField] private int _initialHealth;

	public int InitialHealth => _initialHealth;

}
