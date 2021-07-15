using UnityEngine;
using TMPro;

public class UICreditsScroller : MonoBehaviour
{
	[SerializeField] private InputReader inputReader;


	public TextMeshProUGUI textCredits;
	public RectTransform mask;
	[SerializeField,Tooltip("Set speed of a scrolling effect")]private float _speed = 5f;
	[SerializeField]private float speed = 5f;

	private float expectedFinishingPoint;

	// Start is called before the first frame update
    void Start()
    {
		speed = _speed;
		Invoke("offsetStart",0.01f);
		inputReader.moveEvent += OnMove;
	}

    // Update is called once per frame
    void Update()
    {
		if (textCredits.rectTransform.position.y < expectedFinishingPoint)
		{
			textCredits.rectTransform.position = new Vector2(textCredits.transform.position.x, textCredits.rectTransform.position.y + speed * Time.deltaTime);
		}
	}
	private void offsetStart() {
		inputReader.EnableGameplayInput();
		textCredits.transform.position = new Vector2(textCredits.transform.position.x, -(textCredits.rectTransform.sizeDelta.y / 2));
		expectedFinishingPoint = textCredits.rectTransform.sizeDelta.y + mask.sizeDelta.y + textCredits.transform.position.y;
	}
	private void OnMove(Vector2 direction)
	{
		Debug.Log("y: " + direction.y);
		if (direction.y == 0f)
		{
			speed = _speed;
		}
		else
			if (direction.y > 0f)
			{
			speed = speed * 2;
			}
			else
			{
				speed = -_speed;
			}
	}
}
