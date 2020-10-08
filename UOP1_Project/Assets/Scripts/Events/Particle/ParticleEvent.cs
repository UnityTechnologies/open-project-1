using System.Collections;
using System.Collections.Generic;
using AV.Logic;
using UnityEngine;


public abstract class ParticleEvent : StateAction
{
    public abstract void Play(ParticleSystem particle);
    
    protected override void OnUpdate()
    {
        Play(GetComponent<ParticleSystem>());
    }
}
