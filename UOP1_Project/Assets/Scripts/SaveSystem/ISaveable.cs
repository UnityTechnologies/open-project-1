using System.Collections.Generic;

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
