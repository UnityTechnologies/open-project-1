using UnityEditor;
using UnityEngine;

namespace UOP1.StateMachine.Editor
{
	internal static class ContentStyle
	{
		internal static Color DarkGray { get; private set; }
		internal static Color LightGray { get; private set; }
		internal static Color Focused { get; private set; }
		internal static Color ZebraDark { get; private set; }
		internal static Color ZebraLight { get; private set; }
		internal static RectOffset Padding { get; private set; }
		internal static RectOffset Margin { get; private set; }
		internal static GUIStyle BoldCentered { get; private set; }
		internal static GUIStyle StateListStyle { get; private set; }
		internal static GUIStyle WithPadding { get; private set; }
		internal static GUIStyle WithPaddingAndMargins { get; private set; }

		private static bool _initialised = false;

		[InitializeOnLoadMethod]
		internal static void Initialize()
		{
			if (_initialised)
				return;

			_initialised = true;

			DarkGray = new Color(0.7f, 0.7f, 0.7f);
			LightGray = new Color(0.8f, 0.8f, 0.8f);
			ZebraDark = new Color(0.1f, 0.5f, 0.9f, 0.1f);
			ZebraLight = new Color(0.8f, 0.8f, 0.9f, 0.1f);
			Focused = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			Padding = new RectOffset(5, 5, 5, 5);
			Margin = new RectOffset(8, 8, 8, 8);
			WithPadding = new GUIStyle { padding = Padding };
			WithPaddingAndMargins = new GUIStyle { padding = Padding, margin = Margin };

			BoldCentered = new GUIStyle { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
			StateListStyle = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				fontSize = 12,
				margin = Margin
			};
		}
	}
}
