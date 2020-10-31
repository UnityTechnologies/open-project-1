using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
	public Actor Actor;
	[TextArea(3, 3)]
	public string Sentence;
}
