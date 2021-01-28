using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Audio
{
	/// <summary>
	/// Class to activate AudioCues when a GameObject (i.e. the Player) enters the trigger Collider on this same GameObject.
	/// This component is mostly used for testing purposes.
	/// </summary>
	[RequireComponent(typeof(AudioCue))]
	public class AudioCueOnTriggerEnterTest : MonoBehaviour
	{
		[SerializeField] private bool _isInstantStop = true;
		[SerializeField] private string _tagToDetect = "Player";

		private AudioCue audioCue;

		private void Awake()
		{
			audioCue = GetComponent<AudioCue>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(_tagToDetect))
				audioCue.PlayAudioCue();
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag(_tagToDetect))
			{
				if (_isInstantStop)
					audioCue.StopAudioCue();
				else
					audioCue.FinishAudioCue();
			}
		}
	}
}
