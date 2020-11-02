using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnLocation : MonoBehaviour
{
	[Tooltip("distance above clicked point")]
	[SerializeField] float _verticalOffset = 0.2f;
	//[Tooltip("align spawn location with scene viewport rotation")]
	//[SerializeField] bool _rotateToView = true;

	private Vector3 _spawnPosition;
	private bool _displaySpawnPosition = false;

	private delegate void ButtonAction();
	private ButtonAction myButtonAction;

	private void OnDrawGizmos()
	{
		if (_displaySpawnPosition)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(_spawnPosition, Vector3.one * 0.5f);
		}
	}

	void OnMouseClick(SceneView scene)
	{
		Event c = Event.current;

		Vector3 mousePos = c.mousePosition;
		float ppp = EditorGUIUtility.pixelsPerPoint;
		mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
		mousePos.x *= ppp;

		Ray ray = scene.camera.ScreenPointToRay(mousePos);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			_spawnPosition = hit.point + Vector3.up * _verticalOffset;

			if (c.type == EventType.MouseDown && c.button == 0) // wait for L mouse button down
			{
				myButtonAction(); // perform delegate action
				SceneView.duringSceneGui -= OnMouseClick; // unsubscribe, stop looking for points
				_displaySpawnPosition = false;
			}
		}
		//e.Use(); // idk what Use() does
	}

	// button functions

	public void SetSpawnLocationAtCursor()
	{
		print("left click to set this Spawn Location");
		myButtonAction = SetTransform;
		_displaySpawnPosition = true;
		SceneView.duringSceneGui += OnMouseClick;
	}

	public void PlayGameAtCursor() // idea for 2nd button
	{
		print("left click to enter Play Mode at this Location");
		myButtonAction = PlayGame;
		_displaySpawnPosition = true;
		SceneView.duringSceneGui += OnMouseClick;
	}

	// delegate actions

	private void SetTransform()
	{
		transform.position = _spawnPosition;
		print("Spawn Location set at " + _spawnPosition);
	}

	private void PlayGame() { }
}