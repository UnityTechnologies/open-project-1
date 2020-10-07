using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UOP1.Dialogue;

namespace UOP1.Cutscene
{
	public class CutsceneManager : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private TimelineAsset m_timeline;
#pragma warning restore 649

		private PlayableDirector m_director;

		private void Awake()
		{
			// TODO this used to create an instance of the PseudoDialogueSystem.
			// Normally this would be created by the Game Manager.
			PseudoDialogueSystem dialogueSystem = new PseudoDialogueSystem();

			m_director = GetComponent<PlayableDirector>();
		}

		private void Start()
		{
			PlayCutscene(m_timeline);
		}

		public void PlayCutscene(TimelineAsset cutscene)
		{
			// TODO block player input, keeping dialogue input enabled

			m_director.playableAsset = cutscene;
			m_director.Evaluate();
			m_director.Play();
			m_director.stopped += OnCutsceneCompleted;
		}

		private void OnCutsceneCompleted(PlayableDirector director)
		{
			director.stopped -= OnCutsceneCompleted;

			// TODO return to normal gameplay
		}
	}
}
