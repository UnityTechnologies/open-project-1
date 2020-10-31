using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueControlMix : PlayableBehaviour
{
	private const double PauseThreshold = 0.1d;

    public List<double> ClipsEndTime;
    public List<double> ClipsStartTime;

    // Temp data. 
    private CutsceneManager _cutsceneManager; 
    private bool _showDialogueBox;
    private int _inputCount; 

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
		if(Application.isPlaying == true)
		{  
			// Default state
			_cutsceneManager = playerData as CutsceneManager;

			_showDialogueBox = false;

			_inputCount = playable.GetInputCount();
			for (int i = 0; i < _inputCount; i++)
			{
				float inputWeight = playable.GetInputWeight(i);

				if (inputWeight > 0f)
				{
					ScriptPlayable<DialogueControlBehaviour> inputPlayable = (ScriptPlayable<DialogueControlBehaviour>)playable.GetInput(i);
					DialogueControlBehaviour behaviour = inputPlayable.GetBehaviour();

					_showDialogueBox = true; 

					if (_cutsceneManager._dialogueCounter < behaviour.waitUntil)
					{
						// If we reached end of clip before wait id.
						if (playable.GetTime() >= ClipsEndTime[i] - PauseThreshold)
						{
							_cutsceneManager.PauseTimeline(); 
						}
					}
					else
					{
						// playable.SetTime(ClipsEndTime[i] + PauseThreshold);
						_showDialogueBox = false;	
						_cutsceneManager.director.time = ClipsEndTime[i] + PauseThreshold;
						_cutsceneManager.ResumeTimeline(); 
					}
				}

				_cutsceneManager.OpenDialogueBox(_showDialogueBox);
			}
		}
         
    } 
}
