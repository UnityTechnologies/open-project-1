using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
public class SoundIndicator : MonoBehaviour
{

	[SerializeField] private AudioCueEventChannelSO audioCueEventChannelSO;
	[SerializeField] private InputReader inputReader;
	[SerializeField]private GameObject onScreenIndicator;
	[SerializeField] private GameObject offScreenIndicator;
	[SerializeField] private RectTransform indicatorGroup;
	[SerializeField]public Tuple<AudioCueSO, Vector3, bool, GameObject>[] a;
	public List<Tuple<AudioCueSO,Vector3,bool,GameObject>> screenSounds = new List<Tuple<AudioCueSO, Vector3, bool, GameObject>>();//bool true = onScreen
	public int count=0;
	public float t = 0f;
	// Start is called before the first frame update
	void Awake()
	{
		audioCueEventChannelSO.OnAudioCuePlayRequested += OnSoundPLay;
	}
	private void Update()
	{
		t++;
		count = screenSounds.Count;
		a = screenSounds
			.ToArray();
		OnCameraMove(Vector2.one,false);
	}

	private AudioCueKey OnSoundPLay(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
		AddSound(audioCue, positionInSpace);
		return default;
	}
	private bool OnSoundFinished(AudioCueKey audioCueKey)
	{
		Debug.Log("SoundEnded");
			Vector3 temp;
			if (screenSounds.Any(t => t.Item1 == audioCueKey.AudioCue ))
			{
				temp = screenSounds[audioCueKey.Value].Item2;
				//RemoveSound(audioCueKey.AudioCue,temp);
			}
			
		return true;
	}


	private bool AddSound(AudioCueSO audioCue, Vector3 positionInSpace)
	{
		if (audioCue != null && positionInSpace != null && audioCue.onomatopoeia != "")
		{
			Debug.Log(t);
			if (OnScreen(positionInSpace))
			{
				screenSounds.Add(new Tuple<AudioCueSO, Vector3 ,bool,GameObject>(audioCue, positionInSpace, true,null));
				SpawnOnScreenIndicator(screenSounds.Count - 1);
			}
			else
			{
					screenSounds.Add(new Tuple<AudioCueSO, Vector3, bool,GameObject>(audioCue, positionInSpace,false,null));
					SpawnOffScreenIndicator(screenSounds.Count - 1);

			}
			StartCoroutine(RemoveSound(screenSounds.Count -1,1f));
			return true;
		}
		return false;
	}//Add sound to the list of sounds
	private IEnumerator RemoveSound(int i,float duration)
	{
		yield return new WaitForSeconds(duration);
		if (i < screenSounds.Count)
		{
			
				RemoveScreenIndicator(i);
				screenSounds.RemoveAt(i);


		}
		Debug.LogWarning("You trying to remove soundIndicator from non existing sound. Please chcek what is wrong.");
	}//Remove sound from a list of sounds

	private bool OnScreen(Vector3 positionInSpace) {
		Vector3
			screenPosition = Camera.
			main.
			WorldToScreenPoint(positionInSpace);
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
		float angel = Mathf.Atan2(screenPoint.y,screenPoint.x) * Mathf.Rad2Deg;
		
		//calculating a from y = ax + b    b is 0 becuase we start our line at point 0,0
		float a = screenPoint.y / screenPoint.x;

		float angelBorder = Mathf.Atan2(screenCenter.y, screenCenter.x) * Mathf.Rad2Deg;
		Vector3 borders = screenCenter* 2 * 0.9f;



		float x = 0;
		float y = 0;
		if (-angelBorder <= angel && angel < angelBorder)
		{
			x = borders.x;
			y = a * x;
		}
		else if (angelBorder <= angel && angel < -angelBorder + 180)
		{
			y = borders.y;
			x = y/a;
		}
		else if (-angelBorder + 180 <= angel && angel < angelBorder + 180)
		{
			x = -borders.y;
			y = x*a;
		}
		else if (angelBorder + 180 <= angel && angel < -angelBorder)
		{
			y = -borders.y;
			x = y / a;

		}
		return new Vector2(x,y);
	}


	private void SpawnOnScreenIndicator(int index)
	{
		Debug.Log("Spawnning...");

		//onScreenIndicator.GetComponent<TMPro.TextMeshPro>().text = audioCue.onomatopoeia;
		var objectInd = Instantiate(onScreenIndicator, screenSounds[index].Item2, Quaternion.identity);
		//Debug.Log("index: " + screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true)));
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, true,objectInd);
	}// this spawn an InWoldsIndicator 
	private void RemoveScreenIndicator(int index)
	{
		Destroy(screenSounds[index].Item4);
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, screenSounds[index].Item3, null);

		Debug.Log("Removing...");
	}//this despawn InWorldIndicator

	private void SpawnOffScreenIndicator(int index)
	{
		Debug.Log("Spawning...");

		GameObject temp = Instantiate(offScreenIndicator);
		temp.transform.SetParent(indicatorGroup,false);

		temp.GetComponent<RectTransform>().anchoredPosition = CalculatePostionOfOffScreenIndicator(screenSounds[index].Item2);
		screenSounds[index] = new Tuple<AudioCueSO, Vector3, bool, GameObject>(screenSounds[index].Item1, screenSounds[index].Item2, false,temp);

	}

	private void OnOffScreenCheck(int i)
	{

		if (OnScreen(screenSounds[i].Item2))
		{
			if (screenSounds[i].Item3 == false)//Change from Off to On screen indicator
			{
				RemoveScreenIndicator(i);
				SpawnOnScreenIndicator(i);
			}
		}
		else// if(!OnScreen(positionTemp))
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


	private void OnCameraMove(Vector2 movement,bool isMouseMove)
	{

		for (int i = 0; i < screenSounds.Count; i++)
		{
			OnOffScreenCheck(i);
		}

	}

}

