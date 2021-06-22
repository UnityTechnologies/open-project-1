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
		public Color textColor = Color.black;
		public Color backgroundColor = Color.cyan;
	}
}
