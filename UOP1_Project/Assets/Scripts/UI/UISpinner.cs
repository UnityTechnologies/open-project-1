using UnityEngine;

public class UISpinner : MonoBehaviour
{
	[SerializeField] private float _rotateSpeed = -150f;
	private RectTransform _rectComponent;

	private void Start()
	{
		_rectComponent = GetComponent<RectTransform>();
	}

	private void Update()
	{
		_rectComponent.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
	}
}
