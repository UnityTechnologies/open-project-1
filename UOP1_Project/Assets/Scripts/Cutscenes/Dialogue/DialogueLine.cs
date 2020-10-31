using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
	public Actor Actor;
	public string NameOverride;
	public Sprite FigureOverride;
	[TextArea(3, 3)]
	public string Sentence;
}
