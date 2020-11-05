using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueControlMixerBehaviour : PlayableBehaviour
{
    private CutsceneManager _cutsceneManager = default; 

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
		if(Application.isPlaying == true)
		{  
			// Default state
			_cutsceneManager = playerData as CutsceneManager;

			int inputCount = playable.GetInputCount();

			for (int i = 0; i < inputCount; i++)
			{
				float inputWeight = playable.GetInputWeight(i);

				if (inputWeight > 0f)
				{
					ScriptPlayable<DialogueControlBehaviour> inputPlayable = (ScriptPlayable<DialogueControlBehaviour>)playable.GetInput(i);
					DialogueControlBehaviour behaviour = inputPlayable.GetBehaviour();

					behaviour.DisplayDialogueLine();
				}
			}
		}
         
    } 
}
