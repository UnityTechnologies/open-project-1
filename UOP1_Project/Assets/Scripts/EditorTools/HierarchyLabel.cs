using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Attach this script to a GameOject in the scene to display a label in the hierarchy window.
	/// </summary>
	/// <remarks>
	/// Use the "GameOject/Create Hierarchy Label" menu option to automatically create a new label object. 
	/// </remarks>
	[DisallowMultipleComponent]
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
		[TextArea, Tooltip("Details about this label's purpose and use.")]
		[SerializeField] private string _labelDescription;

		public string Text => _sharedData ? _sharedData.text : _text;

		public Color TextColor => _sharedData ? _sharedData.textColor : _textColor;

		public Color BackgroundColor => _sharedData ? _sharedData.backgroundColor : _backgroundColor;

		private void Reset()
		{
			// Set label text to object name.
			_text = gameObject.name;
		}

#endif

	}
}
