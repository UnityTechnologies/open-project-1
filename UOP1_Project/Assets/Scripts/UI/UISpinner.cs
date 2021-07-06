using UnityEngine;

public class UISpinner : MonoBehaviour
{
	[SerializeField] private float _rotateSpeed = -150f;
	private RectTransform rectComponent;

	private void Start()
	{
		rectComponent = GetComponent<RectTransform>();
	}

	private void Update()
	{
		rectComponent.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
	}
}
