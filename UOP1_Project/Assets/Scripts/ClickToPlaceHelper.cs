using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("UOP1/Tools/Click to Place")]
public class ClickToPlaceHelper : MonoBehaviour
{
	[Tooltip("Vertical offset above the clicked point. Useful to avoid spawn points to be directly ON the geometry which might cause issues.")]
	[SerializeField] float _verticalOffset = 0.1f;

	private Vector3 _spawnPosition;
	private bool _displaySpawnPosition = false;

	private delegate void ButtonAction();
	private ButtonAction myButtonAction;

	private void OnDrawGizmos()
	{
		if (_displaySpawnPosition)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(_spawnPosition, Vector3.one * 0.3f);
		}
	}

	void OnMouseClick(SceneView scene)
	{
		Event currentGUIEvent = Event.current;

		Vector3 mousePos = currentGUIEvent.mousePosition;
		float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
		mousePos.y = scene.camera.pixelHeight - mousePos.y * pixelsPerPoint;
		mousePos.x *= pixelsPerPoint;

		Ray ray = scene.camera.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			_spawnPosition = hit.point + Vector3.up * _verticalOffset;

			if (currentGUIEvent.type == EventType.MouseMove)
			{
				HandleUtility.Repaint();
			}
			if (currentGUIEvent.type == EventType.MouseDown
				&& currentGUIEvent.button == 0) // Wait for Left mouse button down
			{
				myButtonAction();
				SceneView.duringSceneGui -= OnMouseClick;
				_displaySpawnPosition = false;

				currentGUIEvent.Use(); // This consumes the event, so that other controls/buttons won't be able to use it
			}
		}
	}

	public void SetSpawnLocationAtCursor()
	{
		Debug.Log("Use the LMB to position this object");
		myButtonAction = SetTransform;
		_displaySpawnPosition = true;
		SceneView.duringSceneGui += OnMouseClick;
	}

	/// <summary>
	/// The delegate called when the mouse is clicked in the viewport
	/// </summary>
	private void SetTransform()
	{
		transform.position = _spawnPosition;
		Debug.Log("Object moved to " + _spawnPosition);
	}
}
