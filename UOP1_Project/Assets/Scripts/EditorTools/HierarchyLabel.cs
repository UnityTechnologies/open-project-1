using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Attach this script to an empty GameOject in the scene to display a label in the hierarchy window.
	/// </summary>
	/// <remarks>
	/// Use the "GameOject/Create Hierarchy Label" menu option to simplify the creation process.
	/// </remarks>
	[ExecuteInEditMode]
	public class HierarchyLabel : MonoBehaviour
	{

#if UNITY_EDITOR

		[SerializeField] private string _text = "New Label";
		[ColorUsage(false, false)]
		[SerializeField] private Color _textColor = Color.black;
		[ColorUsage(false, false)]
		[SerializeField] private Color _backgroundColor = Color.cyan;
		[Tooltip("Create new label data in project by right-click > Create > Tooling > Hierarchy Label Data.")]
		[SerializeField] private HierarchyLabelData _sharedData;

		public string Text
		{
			get => _sharedData ? _sharedData.text : _text;
		}

		public Color TextColor
		{
			get => _sharedData ? _sharedData.textColor : _textColor;
		}

		public Color BackgroundColor
		{
			get => _sharedData ? _sharedData.backgroundColor : _backgroundColor;
		}

		private void OnValidate()
		{
			gameObject.name = _text + " (Label)";
		}

		private void Reset()
		{
			LockTransform();
			// Exclude label object from builds.
			transform.tag = "EditorOnly";
			// Create label text from object name.
			_text = gameObject.name;
		}

		private void OnDestroy()
		{
			UnlockTransform();
		}

		private void LockTransform()
		{
			// Disable transform editing.
			transform.hideFlags = HideFlags.NotEditable;
			// Reset transform values.
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}

		private void UnlockTransform()
		{
			transform.tag = "Untagged";
			// Enable transform editing.
			transform.hideFlags = HideFlags.None;
		}

#endif

	}
}
