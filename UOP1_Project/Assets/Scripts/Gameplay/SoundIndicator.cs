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

	List<Tuple<AudioCueSO,Vector3,int,bool>> screenSounds = new List<Tuple<AudioCueSO, Vector3, int, bool>>();//bool true = onScreen
	List<Tuple<GameObject, int>> onScreenIndicators = new List<Tuple<GameObject,int>>();
	List<Tuple<GameObject, int>> offScreenIndocators = new List<Tuple<GameObject, int>>();

	// Start is called before the first frame update
	void Awake()
	{
		audioCueEventChannelSO.OnAudioCuePlayRequested += OnSoundPLay;
		audioCueEventChannelSO.OnAudioCueFinishRequested += OnSoundFinished;
		audioCueEventChannelSO.OnAudioCueStopRequested += OnSoundFinished;

		//inputReader.cameraMoveEvent += OnCameraMove;
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
			if (screenSounds.Any(t => t.Item1 == audioCueKey.AudioCue && t.Item3 == audioCueKey.Value))
			{
				temp = screenSounds[audioCueKey.Value].Item2;
				RemoveSound(audioCueKey.AudioCue,temp,audioCueKey.Value);
			}
			
		return true;
	}


	private bool AddSound(AudioCueSO audioCue, Vector3 positionInSpace)
	{
		if (audioCue != null && positionInSpace != null )//&& audioCue.onomatopoeia != "")
		{
			int index = screenSounds.Count;
			if (OnScreen(positionInSpace))
			{
				screenSounds.Add(new Tuple<AudioCueSO, Vector3 , int,bool>(audioCue, positionInSpace, index,true));
				SpawnOnScreenIndicator(audioCue, positionInSpace, index);
			}
			else
			{
				screenSounds.Add(new Tuple<AudioCueSO, Vector3, int,bool>(audioCue, positionInSpace, index,false));

			}


			return true;
		}
		return false;
	}//Add sound to the list of sounds
	private bool RemoveSound(AudioCueSO audioCue, Vector3 positionInSpace, int index)
	{
		if (screenSounds.Any(t => t.Item1 == audioCue && t.Item2 == positionInSpace && t.Item3 == index))
		{
			
			if (OnScreen(positionInSpace))
			{
				screenSounds.Remove(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index,true));
				RemoveOnScreenIndicator(audioCue,positionInSpace,index);
			}
			else
			{
				screenSounds.Remove(new Tuple<AudioCueSO, Vector3 ,int,bool>(audioCue, positionInSpace, index,false));

			}


			return true;
		}
		Debug.LogWarning("You trying to remove soundIndicator from non existing sound. Please chcek what is wrong.");
		return false;
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
		Camera camera = Camera.current;
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
		Vector3 borders = screenCenter * 0.9f;
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


	private void SpawnOnScreenIndicator(AudioCueSO audioCue, Vector3 positionInSpace,int index)
	{
		Debug.Log("Spawnning...");

		//onScreenIndicator.GetComponent<TMPro.TextMeshPro>().text = audioCue.onomatopoeia;
		var objectInd = Instantiate(onScreenIndicator, positionInSpace, Quaternion.identity);
		//Debug.Log("index: " + screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true)));
		onScreenIndicators.Add(new Tuple<GameObject,int>( objectInd, screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true))));
	}// this spawn an InWoldsIndicator 
	private void RemoveOnScreenIndicator(AudioCueSO audioCue, Vector3 positionInSpace, int index)
	{
		int tempInt = screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true));
		foreach (var item in onScreenIndicators)
		{
			if (tempInt == item.Item2)
			{
				Destroy(item.Item1);
				return;
			}
		}
	}//this despawn InWorldIndicator

	private void SpawnOffScreenIndicator(AudioCueSO audioCue, Vector3 positionInSpace, int index)
	{
		Debug.Log("Spawning...");

		GameObject temp = Instantiate(offScreenIndicator);
		temp.transform.SetParent(transform,false);

		temp.GetComponent<RectTransform>().anchoredPosition = CalculatePostionOfOffScreenIndicator(positionInSpace);
		offScreenIndocators.Add(new Tuple<GameObject, int>(temp, screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true))));
	}
	private void RemoveOffScreenIndicator(AudioCueSO audioCue, Vector3 positionInSpace, int index)
	{
		int tempInt = screenSounds.IndexOf(new Tuple<AudioCueSO, Vector3, int, bool>(audioCue, positionInSpace, index, true));
		foreach (var item in offScreenIndocators)
		{
			if (tempInt == item.Item2)
			{
				Destroy(item.Item1);
				return;
			}
		}
	}

	private void OnOffScreenCheck(int i)
	{
		AudioCueSO audioCueTemp = screenSounds[i].Item1;
		Vector3 positionTemp = screenSounds[i].Item2;
		int indexTemp = screenSounds[i].Item3;

		if (OnScreen(positionTemp))
		{
			if (screenSounds[i].Item4 != true)//Change from Off to On screen indicator
			{
				screenSounds[i] = new Tuple<AudioCueSO, Vector3, int, bool>(audioCueTemp, positionTemp, indexTemp, true);
				RemoveOffScreenIndicator(audioCueTemp, positionTemp, indexTemp);
				SpawnOnScreenIndicator(audioCueTemp, positionTemp, indexTemp);
			}
		}
		else if(!OnScreen(positionTemp))
		{
			if (screenSounds[i].Item4 != false)//Change from On to Off screen indicator
			{
				screenSounds[i] = new Tuple<AudioCueSO, Vector3, int, bool>(audioCueTemp, positionTemp, indexTemp, false);
				RemoveOnScreenIndicator(audioCueTemp, positionTemp, indexTemp);
				SpawnOffScreenIndicator(audioCueTemp, positionTemp, indexTemp);
			}
			else //recalculate postiotn
			{
				foreach (var item in offScreenIndocators)
				{
					if (i == item.Item2)
					{
						item.Item1.GetComponent<RectTransform>().anchoredPosition = CalculatePostionOfOffScreenIndicator(screenSounds[i].Item2);
						break;
					}
				}
					
			}
		}
	}//This recalculate status of IN/Out of screen


	private void OnCameraMove(Vector2 movement,bool isMouseMove)
	{
		Debug.Log("CameraMOving");

		for (int i = 0; i < screenSounds.Count; i++)
		{
			OnOffScreenCheck(i);
		}

	}

}

