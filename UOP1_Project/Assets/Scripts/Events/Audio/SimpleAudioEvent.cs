using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Events/Audio Event")]
    public class SimpleAudioEvent : AudioEvent
    {
        public AudioClip[] clips;

        [MinMax(0, 1)]
        public Vector2 volumeRange;
        [MinMax(0, 3)]
        public Vector2 pitchRange;
        
        public override void Play(AudioSource source)
        {
            if (clips.Length == 0)
                return;
            var clip = clips[Random.Range(0, clips.Length)];
            var volume = Random.Range(volumeRange.x, volumeRange.y);
            var pitch = Random.Range(pitchRange.x, pitchRange.y);

            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
        }
    }
}