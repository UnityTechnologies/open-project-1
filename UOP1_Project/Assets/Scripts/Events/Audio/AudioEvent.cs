using System.Collections;
using System.Collections.Generic;
using AV.Logic;
using UnityEngine;

public abstract class AudioEvent : StateAction
{
    public abstract void Play(AudioSource source);
    
    protected override void OnUpdate()
    {
        Play(GetComponent<AudioSource>());
    }
}
