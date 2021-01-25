using System.Collections.Generic;
using UnityEngine;

// This class is essentially the phoneme texture "alphabet" in use. In other words, how the sounds the character
// makes are translated into the final mouth texture.
[CreateAssetMenu(menuName = "Phonemes/New Phoneme Set")]
public class PhonemeSetSO : ScriptableObject
{
	public List<Phoneme> Phonemes = new List<Phoneme>();

	// Dictionary used for efficient runtime lookup
	private Dictionary<string, Texture2D> _phonemeDictionary = new Dictionary<string, Texture2D>();

	public bool _init = false;

	// Initialize the phoneme dictionary, which associates phoneme codes with mouth textures
	public void Initialize()
	{
		_phonemeDictionary.Clear();

		foreach (Phoneme p in Phonemes)
		{
			foreach (string s in p.Codes)
			{
				_phonemeDictionary.Add(s, p.MouthShape);
			}
		}

		_init = true;
	}

	// This function uses the phoneme dictionary to look up and return the corresponding mouth
	// texture for a given phoneme code.
	public Texture2D GetMouthShape(string phonemeKey)
	{
		if (!_init)
			Initialize();

		Texture2D mouthShape = null;

		if (_phonemeDictionary.TryGetValue(phonemeKey, out mouthShape))
		{
			return mouthShape;
		}

		return null;
	}
}
