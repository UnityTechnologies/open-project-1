using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
public class SoundIndicator : MonoBehaviour
{

	[SerializeField] private AudioCueEventChannelSO audioCueEventChannelSO;
	[SerializeField] private InputReader inputReader;
	[SerializeField]private GameObject onScreenIndicator;
	[SerializeField] private GameObject offScreenIndicator;
	[SerializeField] private RectTransform indicatorGroup;
	[SerializeField] private Transform indicatorOnScreenGroup;
	private GameObject player;
	public Dictionary<int,Tuple<AudioCueSO,Vector3,bool,GameObject>> screenSounds = new Dictionary<int, Tuple<AudioCueSO, Vector3, bool, GameObject>>();//bool true = onScreen

	public float maxDistance = 10f;
	private  int n = 0;




	// Start is called before the first frame update
	void Awake()
	{
		audioCueEventChannelSO.OnAudioCuePlayRequested += OnSoundPLay;

	}
	private void Start()
	{
		indicatorOnScreenGroup = new GameObject("IndicatorGroup").transform;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	private void Update()
	{
		RecalculateSounds();
	}

	private AudioCueKey OnSoundPLay(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
		AddSound(audioCue, positionInSpace);
		return default;
	}


	private bool AddSound(AudioCueSO audioCue, Vector3 positionInSpace)
	{
		if (audioCue != null && positionInSpace != null )
		{
			if (OnScreen(positionInSpace))
			{
				screenSounds.Add(n,new Tuple<AudioCueSO, Vector3 ,bool,GameObject>(audioCue, positionInSpace, true,null));
				SpawnOnScreenIndicator(n);
			}
			else
			{
					screenSounds.Add(n,new Tuple<AudioCueSO, Vector3, bool,GameObject>(audioCue, positionInSpace,false,null));
					SpawnOffScreenIndicator(n);

			}
			if (!audioCue.looping)// => looping sound will be shown forever
				StartCoroutine(RemoveSound(n, audioCue.GetOnomatopeia()[0].duration));

			n++;
			return true;
		}
		return false;
	}//Add sound to the list of sounds
	private IEnumerator RemoveSound(int i,float duration)
	{
		yield return new WaitForSeconds(duration);
			Destroy(screenSounds[i].Item4);
				screenSounds.Remove(i);

	}//Remove sound from a list of sounds
	#region HelpingFunctions
	private bool OnScreen(Vector3 positionInSpace) {
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(positionInSpace);
		if (screenPosition.z > 0)
		{
			if (screenPosition.x > 0 && screenPosition.x < Screen.width  &&
				screenPosition.y > 0 && screenPosition.y < Screen.height)
			{
				return true;
			}
		}
		return false;
	}//check if point in space is on screen. IF is it return true
	private Vector2 CalculatePostionOfOffScreenIndicator(Vector3 point)
	{
		
		Camera camera = Camera.main;
		Vector3 screenPoint = camera.WorldToScreenPoint(point);
		if (screenPoint.z < 0)
		{
			screenPoint *= -1;
		}// if it is behind camera everythink is mirrored

		//To make life easier we calculate center of the screen and subtract it from screenpoint to make center of screen 0,0
		Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
		screenPoint -= screenCenter;
		float angle = Mathf.Atan2(screenPoint.y,screenPoint.x) * Mathf.Rad2Deg;
		
		//calculating a from y = ax + b    b is 0 becuase we start our line at point 0,0
		float a = screenPoint.y / screenPoint.x;

		float angleBorder = Mathf.Atan2(screenCenter.y, screenCenter.x) * Mathf.Rad2Deg;
		Vector3 borders = screenCenter* 2 * 0.9f;

		

		float x = 0;
		float y = 0;
		if (angle < 0)
		{
			angle += 360;
		}
		if (-angleBorder+360 <= angle && angle < 360 || angle > 0 && angle < angleBorder)
		{
			x = borders.x;
			y = a * x;
		}
		else if (angleBorder <= angle && angle < -angleBorder + 180)
		{
			y = borders.y;
			x = y / a;
		}
		else if (-angleBorder + 180 <= angle && angle < angleBorder + 180)
		{
			x = -borders.x;
			y = x * a;
		}
		else if (angleBorder + 180 <= angle && angle < -angleBorder+360)
		{
			y = -borders.y;
			x = y / a;

		}
		
		return new Vector2(x,y); 
	}//return postion on screen
	private void OnOffScreenCheck(int i)//This chceck and recalculate position of one sound
	{
		//Distance Feature
		if (Vector3.Distance(player.transform.position,screenSounds[i].Item2) > maxDistance && screenSounds[i].Item4 != null)
		{
			RemoveScreenIndicator(i);
			return;
		}
		else if (Vector3.Distance(player.transform.position, screenSounds[i].Item2) <= maxDistance && screenSounds[i].Item4 == null)
		{
			if (OnScreen(screenSounds[i].Item2))
				SpawnOnScreenIndicator(i);
			else
				SpawnOffScreenIndicator(i);
			return;
		}
			
		if (OnScreen(screenSounds[i].Item2))
		{
			if (screenSounds[i].Item3 == false)//Change from Off to On screen indicator
			{
				RemoveScreenIndicator(i);
				SpawnOnScreenIndicator(i);
			}
		}
		else
		{

			if (screenSounds[i].Item3 == true)//Change from On to Off screen indicator
			{
				RemoveScreenIndicator(i);
				SpawnOffScreenIndicator(i);
			}
			else if(screenSounds[i].Item4 != null) //recalculate postiotn
			{
				screenSounds[i].Item4.GetComponent<RectTransform>().anchoredPosition = CalculatePostionOfOffScreenIndicator(screenSounds[i].Item2);
			}
		}
	}//This recalculate status of IN/Out of screen

	#endregion

	private GameObject SpawnOnScreenIndicator(int index)
	{
		var objectInd = Instantiate(onScreenIndicator, screenSounds[index].Item2, Quaternion.identity);

		//Code bellow just find all onomatopeias and add them to 1 string
		Onomatopeia[] onomatopeias= screenSounds[index].Item1.GetOnomatopeia();
		string temp = "";
		for (int i = 0; i < onomatopeias.Length; i++)
		{
			if (!string.IsNullOrEmpty(onomatopeias[i].SoundText.TableReference))
				temp += onomatopeias[i].SoundText.GetLocalizedString() + "\n";
		}

		objectInd.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = temp;
		objectInd.transform.SetParent(indicatorOnScreenGroup);
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, true,objectInd);
		return objectInd;
	}// this spawn an InWoldsIndicator 
	private GameObject SpawnOffScreenIndicator(int index)
	{

		GameObject temp = Instantiate(offScreenIndicator);
		temp.transform.SetParent(indicatorGroup,false);

		//Code bellow just find all onomatopeias and add them to 1 string
		Onomatopeia[] onomatopeias = screenSounds[index].Item1.GetOnomatopeia();
		string tempString = "";
		for (int i = 0; i < onomatopeias.Length; i++)
		{
			if (!string.IsNullOrEmpty(onomatopeias[i].SoundText.TableReference))
				tempString += onomatopeias[i].SoundText.GetLocalizedString() + "\n";
		}


		temp.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tempString;
		temp.GetComponent<RectTransform>().anchoredPosition = CalculatePostionOfOffScreenIndicator(screenSounds[index].Item2);
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, false,temp);
		return temp;
	}// this spawn an InCanvasIndicator

	private void RemoveScreenIndicator(int index)
	{
		Destroy(screenSounds[index].Item4);
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, screenSounds[index].Item3, null);

	}//this despawn any Indicator

	private void RecalculateSounds()
	{
		var keys = screenSounds.Keys;
		for (int i = 0; i < keys.Count; i++)
		{
			OnOffScreenCheck(keys.ElementAt(i));
		}

	}//This function is called every frame and it loops through all sounds currently playinh and recalculate postion on them

}

