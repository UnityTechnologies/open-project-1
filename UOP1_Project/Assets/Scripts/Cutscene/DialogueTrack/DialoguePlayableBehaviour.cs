using UOP1.Dialogue;
using UnityEngine;
using UnityEngine.Playables;

namespace UOP1.Cutscene
{
    public class DialoguePlayableBehaviour : PlayableBehaviour
    {
        public PseudoDialogueSO Dialogue;
        public PlayableDirector Director;
        
        private bool isDialoguePlaying;
        private bool isDialogueActioned;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (isDialoguePlaying) return;
            
            isDialoguePlaying = true;
            isDialogueActioned = false;

            if (!Application.isPlaying) return;
            PseudoDialogueSystem.ShowDialogue(Dialogue, OnDialogueActioned);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // Called when the dialogue behaviour has stopped playing (i.e. at the end of the track asset in the Timeline)
            if (!isDialogueActioned)
            {
                Director.Pause();
            }
        }

        private void OnDialogueActioned()
        {
            isDialogueActioned = true;
            Director.Play();
        }
    }
}