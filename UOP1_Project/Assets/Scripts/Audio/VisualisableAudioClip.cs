using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class VisualisableAudioClip
	{
		public AudioClip Clip;
		public Onomatopoeia Onomatopoeia;

		public VisualisableAudioClip(AudioClip clip, Onomatopoeia onomatopoeia)
		{
			Clip = clip;
			Onomatopoeia = onomatopoeia;
		}
	}
}
