using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
	public Actor Actor { get => _actor; }
	public string NameOverride { get => _nameOverride; }
	public Sprite FigureOverride { get => _figureOverride; }
	public string Sentence { get => _sentence; }

	[SerializeField] private Actor _actor;
	[SerializeField] private string _nameOverride;
	[SerializeField] private Sprite _figureOverride;
	[SerializeField] [TextArea(3, 3)] private string _sentence;
}
