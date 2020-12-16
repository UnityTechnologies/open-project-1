using UnityEngine;

/// <summary>
/// This probably isn't needed, but it ensures the databases are loaded.
/// </summary>
[ExecuteInEditMode]
public class DatabaseInitialization : MonoBehaviour
{
	void Awake()
	{
		SceneDatabaseSO.Initialize();
		SelectorSurrogateSO.Initialize();
	}
}
