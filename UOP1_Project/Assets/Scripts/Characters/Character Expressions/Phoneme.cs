using System.Collections.Generic;
using UnityEngine;

// Gives a way of specifying blend shape targets
[System.Serializable]
public class BlendTarget
{
	public string						BlendShape;
	[Range(0.0f, 100.0f)] public float  BlendWeight;
}

// A Phoneme object associates Phoneme codes (ex. "AH") with its corresponding mouth texture or
// blendshape. We can get away with making far fewer mouth textures or blendshapes than there
// are phonemes, so this class allows assigning mutliple phoneme codes to a single Mouth Texture.
// The "Name" property just allows it to show up nicely in the Inspector =).
[System.Serializable]
public class Phoneme
{
	public string			 Name;
	public List<string>		 Codes;
	public Texture2D		 MouthShape;
	public List<BlendTarget> BlendTargets;
}
