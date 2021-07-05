using UnityEngine;

public class SpinningUI : MonoBehaviour
{
	private RectTransform _rectComponent;
	[SerializeField] private float _rotateSpeed = 200f;

	private void Start()
	{
		_rectComponent = GetComponent<RectTransform>();
	}

	private void Update()
	{
		_rectComponent.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
	}
}
