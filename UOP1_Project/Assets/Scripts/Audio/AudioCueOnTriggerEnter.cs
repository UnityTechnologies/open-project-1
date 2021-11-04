using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Audio
{
	/// <summary>
	/// Class to activate AudioCues when a GameObject (i.e. the Player) enters the trigger Collider on this same GameObject.
	/// This component is mostly used for testing purposes.
	/// </summary>
	[RequireComponent(typeof(AudioCue))]
	public class AudioCueOnTriggerEnter : MonoBehaviour
	{
		[SerializeField] private string _tagToDetect = "Player";
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(_tagToDetect))
				GetComponent<AudioCue>().PlayAudioCue();
		}
	}
}
