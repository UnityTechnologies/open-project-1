using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Particle Event")]
public class SimpleParticleEvent : ParticleEvent
{
    public enum PlayState
    {
        Restart,
        Play,
        Stop,
    }

    public PlayState playState;
    
    public override void Play(ParticleSystem particle)
    {
        switch (playState)
        {
            case PlayState.Restart:
                particle.time = 0;
                particle.Play();
                break;
            
            case PlayState.Play:
                particle.Play();
                break;
            
            case PlayState.Stop:
                particle.Stop();
                break;
        }
    }
}
