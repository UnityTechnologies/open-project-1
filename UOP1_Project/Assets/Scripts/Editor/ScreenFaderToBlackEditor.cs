using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenFaderToBlack))]
public class ScreenFaderToBlackEditor : Editor
{
	ScreenFaderToBlack fader;
	private void OnEnable()
	{
		fader = (ScreenFaderToBlack)target;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("FadeToBlack"))
		{
			fader.FadeScreenToBlack(true);
		}
		if (GUILayout.Button("FadeFromBlack"))
		{
			fader.FadeScreenToBlack(false);
		}
	}
}
