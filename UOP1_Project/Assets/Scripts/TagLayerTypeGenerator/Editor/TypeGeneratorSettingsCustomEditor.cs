using UnityEditor;
using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Custom inspector for <see cref="TypeGeneratorSettings" />.</summary>
	[CustomEditor(typeof(TypeGeneratorSettings))]
	internal sealed class TypeGeneratorSettingsCustomEditor : UnityEditor.Editor
	{
		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
			EditorGUI.BeginDisabledGroup(!TagTypeGenerator.Generator.CanGenerate());
			if (GUILayout.Button("Regenerate Tag Type File")) TagTypeGenerator.Generator.GenerateFile();
			EditorGUI.EndDisabledGroup();
			EditorGUI.BeginDisabledGroup(!LayerTypeGenerator.Generator.CanGenerate());
			if (GUILayout.Button("Regenerate Layer Type File")) LayerTypeGenerator.Generator.GenerateFile();
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.LabelField("Open", EditorStyles.boldLabel);
			if (GUILayout.Button("Project Settings")) SettingsService.OpenProjectSettings(TypeGeneratorSettingsProvider.ProjectSettingPath);
			if (GUILayout.Button("Tags and Layers")) SettingsService.OpenProjectSettings("Project/Tags and Layers");
		}
	}
}
