/// <summary>
/// <b>DO NOT DELETE THIS FILE !!!!!!</b><br/>
/// This class contains all the variables that will be serialized and saved to a binary file.<br/>
/// Can be considered as a save file structure or format.
/// </summary>
[System.Serializable]
public class Save {
	// This is test data, written accodring to TestScript.cs class
	// This will change according to whatever data that needs to be stored

	// The variables need to be public, else we would have to write trivial getter/setter functions.

	public int _testInteger = default;
	public float _testFloat = default;
	public bool _testBool = default;
}
