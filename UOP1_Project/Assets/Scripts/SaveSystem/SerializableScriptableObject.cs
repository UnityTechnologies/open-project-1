using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializableScriptableObject : ScriptableObject
{
	private string _serializableGuid;
	public string SerializableGuid => _serializableGuid;

	void OnValidate()
	{
#if UNITY_EDITOR
		var path = AssetDatabase.GetAssetPath(this);
		_serializableGuid = AssetDatabase.AssetPathToGUID(path);
#endif
	}
}
