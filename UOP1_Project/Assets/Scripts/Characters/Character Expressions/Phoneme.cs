using System.Collections.Generic;
using UnityEngine;

// A Phoneme object associates a Phoneme code (ex. "AH") with its corresponding mouth texture.
// We can get away with making far fewer mouth textures than there are phonemes, so this class
// allows assigning mutliple phoneme codes to a single Mouth Texture. The "Name" property just
// allows it to show up nicely in the Inspector.
[System.Serializable]
public class Phoneme
{
	public string Name;
	public List<string> Codes;
	public Texture2D MouthShape;
}
