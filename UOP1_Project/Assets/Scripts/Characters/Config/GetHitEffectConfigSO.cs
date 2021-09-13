using UnityEngine;

[CreateAssetMenu(fileName = "GetHitEffectConfig", menuName = "EntityConfig/Get Hit Effect Config")]
public class GetHitEffectConfigSO : ScriptableObject
{
	[SerializeField] private Color _getHitFlashingColor = default;
	[SerializeField] private float _getHitFlashingDuration = 0.5f;
	[SerializeField] private float _getHitFlashingSpeed = 3.0f;

	public Color GetHitFlashingColor => _getHitFlashingColor;
	public float GetHitFlashingDuration => _getHitFlashingDuration;
	public float GetHitFlashingSpeed => _getHitFlashingSpeed;
}
