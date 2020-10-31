using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
	public Sprite Figure;
	public string ActorName;
	[TextArea(3, 3)]
	public string Sentence;
}
