using UnityEngine;
using TMPro;

public class UICreditsRoller : MonoBehaviour
{


	[SerializeField,Tooltip("Set speed of a rolling effect")]private float _speedPreset = 100f;//normal rolling speed
	[SerializeField,Tooltip("This is actuall speed of rolling")]private float speed = 100f;//actual speed of rolling
	public bool rollAgain = false;

	[Header("References")]
	[SerializeField] private InputReader inputReader;
	[SerializeField]private VoidEventChannelSO creditsRollEndEvent= default;
	[SerializeField]private RectTransform textCredits;
	[SerializeField]private RectTransform mask;
	[SerializeField] private UICredits _uiCredits;
	
	private float expectedFinishingPoint;
	// Start is called before the first frame update
    void Start()
    {
		speed = _speedPreset;
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

		//This make rolling effect
		if (textCredits.anchoredPosition.y < expectedFinishingPoint)
		{
			textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, textCredits.anchoredPosition.y + speed * Time.deltaTime);
		}
		else if(expectedFinishingPoint != 0)//this happend when rolling reach to end
		{
			RollingEnd();
		}
	}
	private void offsetStart() {
		inputReader.EnableGameplayInput();
		expectedFinishingPoint = (textCredits.rect.height + mask.rect.height) / 2;
		
		textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, -((textCredits.rect.height + mask.rect.height )/ 2 ) );

	}
	private void OnMove(Vector2 direction)
	{
		if (direction.y == 0f)//no horizontal movment
		{
			speed = _speedPreset;
		}
		else if (direction.y > 0f)//upward movment
		{
			speed = speed * 2;
		}
		else//downward movment
		{
			speed = -_speedPreset;
		}
	}
	private void RollingEnd()
	{
		creditsRollEndEvent.RaiseEvent();
		//reset postion of an element
		textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x, -((textCredits.rect.height + mask.rect.height) / 2));
		if(rollAgain == false)
		{
			//close credits
			_uiCredits.CloseCreditsScreen();
		}
	}
}
