using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class VisualisableAudioClip
	{
		public AudioClip Clip;
		public Caption Caption;

		public VisualisableAudioClip(AudioClip clip, Caption caption)
		{
			Clip = clip;
			Caption = caption;
		}
	}
}
