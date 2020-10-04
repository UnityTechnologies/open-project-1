using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UOP1.Dialogue;

namespace UOP1.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TimelineAsset timeline;
#pragma warning restore 649

        private PlayableDirector director;
        
        private void Awake()
        {
            // TODO this used to create an instance of the PseudoDialogueSystem.
            // Normally this would be created by the Game Manager.
            PseudoDialogueSystem dialogueSystem = new PseudoDialogueSystem();

            director = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            PlayCutscene(timeline);
        }

        public void PlayCutscene(TimelineAsset cutscene)
        {
            // TODO block player input, keeping dialogue input enabled
            
            director.playableAsset = cutscene;
            director.Evaluate();
            director.Play();
            director.stopped += OnCutsceneCompleted;
        }

        private void OnCutsceneCompleted(PlayableDirector director)
        {
            director.stopped -= OnCutsceneCompleted;
            
            // TODO return to normal gameplay
        }
    }
}