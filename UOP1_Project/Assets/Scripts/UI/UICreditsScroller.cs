using UnityEngine;
using TMPro;

public class UICreditsScroller : MonoBehaviour
{


	public TextMeshProUGUI textCredits;
	public RectTransform mask;
	[SerializeField,Tooltip("Set speed of a scrolling effect")]private float speed = 5f;


	private float expectedFinishingPoint;

	// Start is called before the first frame update
    void Start()
    {
		Invoke("offsetStart",0.01f);
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
		textCredits.transform.position = new Vector2(textCredits.transform.position.x, -(textCredits.rectTransform.sizeDelta.y / 2));
		expectedFinishingPoint = textCredits.rectTransform.sizeDelta.y + mask.sizeDelta.y + textCredits.transform.position.y;
	}
}
