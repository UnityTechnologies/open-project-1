using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

public class DialogMixerBehaviour : PlayableBehaviour
{
    private const float k_StopThreshold = 0.1f;

    private DialogBinder m_dialogBinder;
    private bool m_initialized;
    private int m_currentClipIndex;

    private enum ClipState
    {
        Pending, Started, Ended
    }

    private class ClipData
    {
        public ScriptPlayable<DialogBehaviour> Input;
        public ClipState state;
        public Range Range;
    }
    private ClipData[] m_clipsData;
    private TimelineClip[] m_clips;

    private struct Range
    {
        public double Start;
        public double End;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        CreateClipData(playable);
        base.OnBehaviourPlay(playable, info);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        CalculateCurrentClip(playable);

        ClipData data = m_clipsData[m_currentClipIndex];
        ScriptPlayable<DialogBehaviour> playableInput = data.Input;
        DialogBehaviour input = playableInput.GetBehaviour();

        ProcessCurrentClip(playable, playerData, input, data);
    }

    private void CalculateCurrentClip(Playable playable)
    {
        //TODO not very efficient, is there a way to find the current input?
        double currentTime = playable.GetTime();
        for (int i = 0; i < m_clipsData.Length; i++)
        {
            if (currentTime >= m_clipsData[i].Range.Start && currentTime < m_clipsData[i].Range.End)
            {
                m_currentClipIndex = i;
                m_clipsData[m_currentClipIndex].state = ClipState.Pending;
                break;
            }
        }
    }

    private void ProcessCurrentClip(Playable playable, object playerData, DialogBehaviour input, ClipData clipData)
    {
        double currentTime = playable.GetTime();
        if (currentTime > 0f)
        {
            if (clipData.state == ClipState.Pending)
            {
                clipData.state = ClipState.Started;
                (playerData as DialogBinder)?.SetDialog(input.dialogID);
            }
            
            if(clipData.state == ClipState.Started)
            {
                if (currentTime >= m_clipsData[m_currentClipIndex].Range.End - k_StopThreshold)
                {
                    clipData.state = ClipState.Ended;
                }
            }

            if (clipData.state == ClipState.Ended)
            {
                if (NextClipIndexInBounds(m_currentClipIndex + 1))
                {
                    m_currentClipIndex++;
                    playable.SetTime(m_clipsData[m_currentClipIndex].Range.Start);
                    if (input.stopGraphOnClipEnd)
                    {
                        playable.GetGraph().Stop();
                    }
                }
            }
        }
    }

    private bool NextClipIndexInBounds(int nextInput)
    {
        return nextInput < m_clipsData.Length;
    }

    private void CreateClipData(Playable playable)
    {
        if (m_clipsData != null)
            return;
        
        int inputCount = playable.GetInputCount();
        m_clipsData = new ClipData[inputCount];
        for (int i = 0; i < inputCount; i++)
        {
            m_clipsData[i] = new ClipData()
            {
                Input = (ScriptPlayable<DialogBehaviour>) playable.GetInput(i),
                Range = new Range()
                {
                    Start = m_clips[i].start,
                    End = m_clips[i].end
                }
            };
            m_currentClipIndex = 0;
        }
    }

    public void SetClips(TimelineClip[] clips)
    {
        m_clips = clips;
    }
}
