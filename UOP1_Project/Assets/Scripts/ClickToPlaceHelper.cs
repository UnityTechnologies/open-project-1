using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("UOP1/Tools/Click to Place")]
public class ClickToPlaceHelper : MonoBehaviour
{
	[Tooltip("Vertical offset above the clicked point. Useful to avoid spawn points to be directly ON the geometry which might cause issues.")]
	[SerializeField] private float _verticalOffset = 0.1f;

	private Vector3 _targetPosition;
	private bool _targeting = false;

	public bool targeting => _targeting;

	private void OnDrawGizmos()
	{
		if (_targeting)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(_targetPosition, Vector3.one * 0.3f);
		}
	}

	public void BeginTargeting()
	{
		_targeting = true;
		_targetPosition = transform.position;
	}

	public void UpdateTargeting(Vector3 spawnPosition)
	{
		_targetPosition = spawnPosition + Vector3.up * _verticalOffset;
	}

	public void EndTargeting()
	{
		_targeting = false;
		transform.position = _targetPosition;
	}
}
