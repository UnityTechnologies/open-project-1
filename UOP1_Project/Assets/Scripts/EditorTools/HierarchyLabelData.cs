using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Stores label data that can be referenced by multiple labels of
	/// the same type in different scenes, to help maintain consistency.
	/// </summary>
	[CreateAssetMenu(menuName = "Tooling/Hierarchy Label Data")]
	public class HierarchyLabelData : ScriptableObject
	{
		public string text = "Label";
		[ColorUsage(false, false)]
		public Color textColor = Color.black;
		[ColorUsage(false, false)]
		public Color backgroundColor = Color.cyan;
		[TextArea, Tooltip("Details about this label's purpose and use.")]
		public string labelDescription;
	}
}
