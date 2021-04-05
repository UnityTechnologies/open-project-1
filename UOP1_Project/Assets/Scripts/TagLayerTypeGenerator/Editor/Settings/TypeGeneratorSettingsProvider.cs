using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UOP1.TagLayerTypeGenerator.Editor.Settings
{
	/// <summary>Settings provider for <see cref="TypeGeneratorSettings" />.</summary>
	internal sealed class TypeGeneratorSettingsProvider : SettingsProvider
	{
		/// <summary>Path to the Project Settings.</summary>
		internal const string ProjectSettingPath = TagsAndLayersProjectSettings + "/Automatic Type Generation";

		/// <summary>Path to the built-in Tags and Layers Manager.</summary>
		private const string TagsAndLayersProjectSettings = "Project/Tags and Layers";

		/// <summary><see cref="TypeGeneratorSettings" /> wrapped in a <see cref="SerializedObject" />.</summary>
		private SerializedObject _settings;

		/// <inheritdoc />
		private TypeGeneratorSettingsProvider(string path, SettingsScope scope) : base(path, scope)
		{
		}

		/// <inheritdoc />
		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			_settings = TypeGeneratorSettings.GetSerializedSettings();
		}

		/// <inheritdoc />
		public override void OnGUI(string searchContext)
		{
			PropertiesGUI(nameof(TypeGeneratorSettings.Tag));
			PropertiesGUI(nameof(TypeGeneratorSettings.Layer));

			EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

			EditorGUI.BeginDisabledGroup(!TagTypeGenerator.Generator.CanGenerate());
			if (GUILayout.Button("Regenerate Tag Type File")) TagTypeGenerator.Generator.GenerateFile();
			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginDisabledGroup(!LayerTypeGenerator.Generator.CanGenerate());
			if (GUILayout.Button("Regenerate Layer Type File")) LayerTypeGenerator.Generator.GenerateFile();
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.LabelField("Open", EditorStyles.boldLabel);
			if (GUILayout.Button("Settings Asset")) Selection.SetActiveObjectWithContext(_settings.targetObject, _settings.context);
			if (GUILayout.Button("Tags and Layers")) SettingsService.OpenProjectSettings(TagsAndLayersProjectSettings);

			_settings.ApplyModifiedPropertiesWithoutUndo();
		}

		private void PropertiesGUI(string property)
		{
			EditorGUILayout.LabelField(property, EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_settings.FindProperty($"{property}.{nameof(TypeGeneratorSettings.Settings.AutoGenerate)}"), Styles.AutoGenerate);
			EditorGUILayout.PropertyField(_settings.FindProperty($"{property}.{nameof(TypeGeneratorSettings.Settings.TypeName)}"), Styles.TypeName);
			EditorGUILayout.PropertyField(_settings.FindProperty($"{property}.{nameof(TypeGeneratorSettings.Settings.FilePath)}"), Styles.FilePath);
			EditorGUILayout.PropertyField(_settings.FindProperty($"{property}.{nameof(TypeGeneratorSettings.Settings.Namespace)}"), Styles.Namespace);
			EditorGUILayout.PropertyField(_settings.FindProperty($"{property}.{nameof(TypeGeneratorSettings.Settings.AssemblyDefinition)}"), Styles.AssemblyDefinition);

			EditorGUILayout.Space();
		}

		/// <summary>Creates the <see cref="SettingsProvider" /> for the Project Settings window.</summary>
		/// <returns>The <see cref="SettingsProvider" /> for the Project Settings window.</returns>
		[SettingsProvider]
		private static SettingsProvider CreateTagClassGeneratorSettingsProvider()
		{
			return new TypeGeneratorSettingsProvider(ProjectSettingPath, SettingsScope.Project)
				{keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()};
		}

		/// <summary>Styles for the <see cref="SettingsProvider" />.</summary>
		private /*readonly*/ struct Styles
		{
			public static readonly GUIContent AutoGenerate = new GUIContent("Auto Generate");
			public static readonly GUIContent TypeName = new GUIContent("Type Name");
			public static readonly GUIContent FilePath = new GUIContent("File Path");
			public static readonly GUIContent Namespace = new GUIContent("Namespace");
			public static readonly GUIContent AssemblyDefinition = new GUIContent("Assembly Definition");
		}
	}
}
