using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;


public interface ISaveable
{

	/// <summary>
	/// <para>The function which needs to be subscribed to the <see cref="SaveSystem.AddToRegistryCallback"/></para>
	/// Include it in a place which only gets executed once to avoid data duplication.<br/>
	/// Like in OnEnable() function of Monobehaviour or ScriptableObject
	/// </summary>
	void AddToSaveRegistry(HashSet<ISaveable> registry);

	/// <summary>
	/// Pure virtual function for saving data to a save file.<br/>
	/// This will comprise the serialization logic.
	/// </summary>
	void Serialize(Save saveFile);

	/// <summary>
	/// Pure virtual function for loading data from a save file.<br/>
	/// This will comprise the deserialziation logic.
	/// </summary>
	void Deserialize(Save saveFile);
}

public class SaveSystem : MonoBehaviour
{
	public delegate void AddToRegistryCallback(HashSet<ISaveable> registry);
	public static AddToRegistryCallback AddToRegistry;

	HashSet<ISaveable> _saveRegistry = default;

	[SerializeField]
	string _fileNameExtension;

	private void Start()
	{
		Debug.Log("SaveSystem start getting called");
		_saveRegistry = new HashSet<ISaveable>();
		AddToRegistry?.Invoke(_saveRegistry);
		LoadGame();
	}

	private void LoadGame()
	{
		FileStream file;

		if(!File.Exists(Application.persistentDataPath + "/save" + _fileNameExtension))
		{
			Debug.LogError("No Save Data found");
			return;

		} else
		{
			file = File.Open(Application.persistentDataPath + "/save" + _fileNameExtension, FileMode.Open);
		}

		BinaryFormatter formatter = new BinaryFormatter();

		Save saveClass = (Save)formatter.Deserialize(file);

		file.Close();

		foreach(ISaveable saveable in _saveRegistry)
		{
			saveable.Deserialize(saveClass);
		}
	}

	private void SaveAndQuit()
	{
		SaveGame();
		Application.Quit();
	}

	private void SaveGame()
	{
		FileStream file;

		if(!File.Exists(Application.persistentDataPath + "/save" + _fileNameExtension))
		{
			file = File.Create(Application.persistentDataPath + "/save" + _fileNameExtension);

		} else
		{
			file = File.Open(Application.persistentDataPath + "/save" + _fileNameExtension, FileMode.Open);
		}

		// A class with name "Save" must exist in the project. It will contain the appropriate save file structure.
		Save saveClass = new Save();

		foreach(ISaveable saveable in _saveRegistry)
		{
			saveable.Serialize(saveClass);
		}

		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(file, saveClass);
		file.Close();
	}

	private void OnDisable()
	{
		SaveAndQuit();
	}

}
