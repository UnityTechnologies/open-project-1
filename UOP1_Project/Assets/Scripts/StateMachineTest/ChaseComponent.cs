using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
	[SerializeField] private Transform _target = default;
	[SerializeField] private float _speed = 10f;

	public Transform Target => _target;

	public void Chase()
	{
		transform.position = Vector3.MoveTowards(
			transform.position,
			_target.position,
			_speed * Time.deltaTime);
	}
}
