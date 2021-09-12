using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "EntityConfig/Health Config")]
public class HealthConfigSO : ScriptableObject
{
	[Tooltip("Initial health")]
	[SerializeField][UnityEngine.Serialization.FormerlySerializedAs("_maxHealth")] private int _initialHealth;

	public int InitialHealth => _initialHealth;

}
