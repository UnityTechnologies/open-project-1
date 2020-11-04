using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueControlBehaviour : PlayableBehaviour
{
	[Tooltip("Pause timeline if it's already in the end of the clip and dialogue counter is still less or equal than WaitUntil")]
    public int WaitUntil;
}
