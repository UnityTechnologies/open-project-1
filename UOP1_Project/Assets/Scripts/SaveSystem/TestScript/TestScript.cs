using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test script showing usage of the save system.<br/>
/// This code corresponds to the code written in <see cref="Save"/> class.<br/>
/// Unfortunately, inspite of all my efforts, I wasn't able to remove all dependencies. 
/// </summary>
public class TestScript : MonoBehaviour, ISaveable
{
	int _testInteger = default;
	float _testFloat = default;
	bool _testBool = default;

	private void OnEnable()
	{
		// This is necessary, otherwise the object won't get added to registry of objects which needs to be serialized
		SaveSystem.AddToRegistry += AddToSaveRegistry;
	}

	private void OnDisable()
	{
		SaveSystem.AddToRegistry -= AddToSaveRegistry;
	}

	public void AddToSaveRegistry(HashSet<ISaveable> registry)
	{
		registry.Add(this);
	}

	public void Serialize(Save saveFile)
	{
		saveFile._testInteger = _testInteger;
		saveFile._testFloat = _testFloat;
		saveFile._testBool = _testBool;
	}

	public void Deserialize(Save saveFile)
	{
		_testInteger = saveFile._testInteger;
		_testFloat = saveFile._testFloat;
		_testBool = saveFile._testBool;
	}
}
