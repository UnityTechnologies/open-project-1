using UnityEngine;
using TMPro;

public class UICreditsScroller : MonoBehaviour
{
	[SerializeField] private InputReader inputReader;


	public RectTransform textCredits;
	public RectTransform mask;
	[SerializeField,Tooltip("Set speed of a scrolling effect")]private float _speed = 5f;//normal scrolling speed
	[SerializeField]private float speed = 5f;//actual speed of scrolling
	[SerializeField]private VoidEventChannelSO creditsScrollEndEvent= default;
	private float expectedFinishingPoint;
	[SerializeField] private UICredits _uiCredits;
	
	public bool scrolAgain = false;
	// Start is called before the first frame update
    void Start()
    {
		speed = _speed;
		Invoke("offsetStart",0.01f);//This offset is needed to get true informations about rectangle and his mask
		inputReader.moveEvent += OnMove;
	}
	private void OnDestroy()
	{
		inputReader.moveEvent -= OnMove;
	}
	// Update is called once per frame
	void Update()
    {

		//This make scrolling effect
		if (textCredits.anchoredPosition.y < expectedFinishingPoint)
		{
			textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, textCredits.anchoredPosition.y + speed * Time.deltaTime);
		}
		else if(expectedFinishingPoint != 0)//this happend when scrolling reach to end
		{
			ScrollingEnd();
		}
	}
	private void offsetStart() {
		inputReader.EnableGameplayInput();
		expectedFinishingPoint = (textCredits.rect.height + mask.rect.height) / 2;
		
		textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, -((textCredits.rect.height + mask.rect.height )/ 2 ) );

	}
	private void OnMove(Vector2 direction)
	{
		if (direction.y == 0f)
		{
			speed = _speed;
		}
		else if (direction.y > 0f)
		{
			speed = speed * 2;
		}
		else
		{
			speed = -_speed;
		}
	}
	private void ScrollingEnd()
	{
		creditsScrollEndEvent.RaiseEvent();
		if (scrolAgain == true)//reset postion of an element
		{
			textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, -((textCredits.rect.height + mask.rect.height) / 2));

		}
		else
		{
			//close credits
			_uiCredits.CloseCreditsScreen();
		}
	}
}
