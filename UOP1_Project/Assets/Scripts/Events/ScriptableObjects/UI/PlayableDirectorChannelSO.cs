using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Events/Playable Director Channel")]
public class PlayableDirectorChannelSO : DescriptionBaseSO
{
	public UnityAction<PlayableDirector> OnEventRaised;
	
	public void RaiseEvent(PlayableDirector playable)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(playable);
	}
}
