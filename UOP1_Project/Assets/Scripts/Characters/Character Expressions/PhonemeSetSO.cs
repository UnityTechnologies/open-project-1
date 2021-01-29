using System.Collections.Generic;
using UnityEngine;

// To specify whether the character uses 2D (textures) or 3D (blendshapes) for mouth expressions
public enum PhonemeType { TwoD, ThreeD }

// This class is essentially the phoneme "alphabet" in use. In other words, how the sounds the character
// makes are translated into the final mouth texture. It applies to characters whose facial expressions
// are controlled via the mainTexture of a material (2D) or blend shapes (3D).
[CreateAssetMenu(menuName = "Phonemes/New Phoneme Set")]
public class PhonemeSetSO : ScriptableObject
{
	public PhonemeType Type;
	public List<Phoneme> Phonemes = new List<Phoneme>(); // The "alphabet"

	// Dictionary used for efficient runtime lookup
	private Dictionary<string, object> _phonemeDictionary = new Dictionary<string, object>();
	private bool _init = false;

	// Initialize the phoneme dictionary, which associates phoneme codes with mouth textures or blend targets
	public void Initialize()
	{
		_phonemeDictionary.Clear();

		foreach (Phoneme p in Phonemes)
		{
			foreach (string s in p.Codes)
			{
				if (Type == PhonemeType.TwoD)
				{
					_phonemeDictionary.Add(s, p.MouthShape);
				}
				else if (Type == PhonemeType.ThreeD)
				{
					_phonemeDictionary.Add(s, p.BlendTargets);
				}
			}
		}

		_init = true;
	}

	// This function uses the phoneme dictionary to look up and return the corresponding mouth
	// texture or blend target for a given phoneme code.
	public object GetMouthShape(string phonemeKey)
	{
		if (!_init)
			Initialize();

		object mouthShape;
		if (_phonemeDictionary.TryGetValue(phonemeKey, out mouthShape))
		{
			return mouthShape;
		}

		return null;
	}
}
