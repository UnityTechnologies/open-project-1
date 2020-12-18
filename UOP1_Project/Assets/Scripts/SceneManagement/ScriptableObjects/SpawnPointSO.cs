using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Events/SpawnPoint")]
public class SpawnPointSO : ScriptableObject
{
	public bool Used;
	public bool SceneChange;
	public Vector3 Position, Rotation;

	public void ExecuteLoad(PointSO data, GameSceneSO loadTarget)
	{
		if (SceneManager.GetActiveScene().name != loadTarget.sceneName)
		{
			SceneChange = true;
		}
		Used = true;
		Position = data.Position;
		Rotation = data.Rotation;
	}
}
