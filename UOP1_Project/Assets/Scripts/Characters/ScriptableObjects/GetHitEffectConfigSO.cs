using UnityEngine;

[CreateAssetMenu(fileName = "GetHitEffectConfig", menuName = "EntityConfig/Get Hit Effect Config")]
public class GetHitEffectConfigSO : ScriptableObject
{
	[Tooltip("Flashing effect color applied when getting hit.")]
	[SerializeField]
	private Color _getHitFlashingColor = default;

	[Tooltip("Flashing effect duration (in seconds).")]
	[SerializeField]
	private float _getHitFlashingDuration = 0.5f;

	[Tooltip("Flashing effect speed (number of flashings during the duration).")]
	[SerializeField]
	private float _getHitFlashingSpeed = 3.0f;

	public Color GetHitFlashingColor => _getHitFlashingColor;
	public float GetHitFlashingDuration => _getHitFlashingDuration;
	public float GetHitFlashingSpeed => _getHitFlashingSpeed;
}
