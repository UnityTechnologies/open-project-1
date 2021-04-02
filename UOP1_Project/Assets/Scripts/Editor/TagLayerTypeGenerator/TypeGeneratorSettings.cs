using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static System.CodeDom.Compiler.CodeGenerator;
using static System.String;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Settings for <see cref="TagTypeGenerator" />.</summary>
	public sealed class TypeGeneratorSettings : ScriptableObject
	{
		/// <summary>Default value for <see cref="Tag" /> <see cref="Settings.TypeName" />.</summary>
		private const string DefaultTagTypeName = "Tag";

		/// <summary>Default value for <see cref="Layer" /> <see cref="Settings.TypeName" />.</summary>
		private const string DefaultLayerTypeName = "Layer";

		/// <summary>Default value for <see cref="Tag" /> <see cref="Settings.FilePath" />.</summary>
		private const string DefaultTagFilePath = "Scripts/Tag.cs";

		/// <summary>Default value for <see cref="Layer" /> <see cref="Settings.FilePath" />.</summary>
		private const string DefaultLayerFilePath = "Scripts/Layer.cs";

		/// <summary>Where to create a new <see cref="TypeGeneratorSettings" /> asset.</summary>
		private const string DefaultSettingsAssetPath = "Assets/TypeGenerationSettings.asset";

		/// <summary>Log errors about invalidate identifiers with this string.</summary>
		private const string InvalidIdentifier = "'{0}' is an invalid identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.";

		/// <summary>Where to start the asset search for settings.</summary>
		private static readonly string[] SearchInFolders = {"Assets"};

		/// <summary>Settings for Tags.</summary>
		[SerializeField] internal Settings Tag;

		/// <summary>Settings for Layers.</summary>
		[SerializeField] internal Settings Layer;

		/// <summary>Reset to default values.</summary>
		private void Reset()
		{
			Tag = new Settings
			{
				TypeName = DefaultTagTypeName,
				FilePath = DefaultTagFilePath
			};

			Layer = new Settings
			{
				TypeName = DefaultLayerTypeName,
				FilePath = DefaultLayerFilePath
			};

			Tag.Namespace = Layer.Namespace = Application.productName.Replace(" ", Empty);
		}

		/// <summary>
		///     This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor
		///     only).
		/// </summary>
		private void OnValidate()
		{
			if (!IsValidLanguageIndependentIdentifier(Tag.TypeName)) Debug.LogErrorFormat(InvalidIdentifier, Tag.TypeName);
			if (!IsNullOrWhiteSpace(Tag.Namespace) && !IsValidLanguageIndependentIdentifier(Tag.Namespace)) Debug.LogErrorFormat(InvalidIdentifier, Tag.Namespace);
			if (IsNullOrWhiteSpace(Tag.FilePath) && !IsValidLanguageIndependentIdentifier(Tag.FilePath)) Debug.LogError("Tag path cannot be empty.");
			if (!IsValidLanguageIndependentIdentifier(Layer.TypeName)) Debug.LogErrorFormat(InvalidIdentifier, Layer.TypeName);
			if (!IsNullOrWhiteSpace(Layer.Namespace) && !IsValidLanguageIndependentIdentifier(Layer.Namespace)) Debug.LogErrorFormat(InvalidIdentifier, Layer.Namespace);
			if (IsNullOrWhiteSpace(Layer.FilePath) && !IsValidLanguageIndependentIdentifier(Layer.FilePath)) Debug.LogError("Layer path cannot be empty.");
		}

		/// <summary>Returns <see cref="InvalidOperationException" /> or creates a new one and saves the asset.</summary>
		/// <returns>The <see cref="TypeGeneratorSettings" /> to use.</returns>
		/// <exception cref="TypeGeneratorSettings">More than one <see cref="TypeGeneratorSettings" /> are in the project.</exception>
		internal static TypeGeneratorSettings GetOrCreateSettings()
		{
			string[] guids = AssetDatabase.FindAssets($"t:{nameof(TypeGeneratorSettings)}", SearchInFolders);

			TypeGeneratorSettings settings;
			switch (guids.Length)
			{
				case 0:
					CreateSettings(out settings);
					break;
				case 1:
					LoadSettings(guids.Single(), out settings);
					break;
				default:
					throw new InvalidOperationException(
						$"There must be only one {nameof(TypeGeneratorSettings)} asset in '{Application.productName}'.\n " +
						$"Found: {Join(", ", guids.Select(AssetDatabase.GUIDToAssetPath))}.");
			}

			return settings;
		}

		/// <summary>Loads <see cref="GUID" /> via <see cref="GUID" />.</summary>
		/// <param name="guid">The <see cref="GUID" /> of the asset.</param>
		/// <param name="settings">The loaded <see cref="TypeGeneratorSettings" />.</param>
		private static void LoadSettings(string guid, out TypeGeneratorSettings settings)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);

			settings = AssetDatabase.LoadAssetAtPath<TypeGeneratorSettings>(path);
		}

		/// <summary>Creates new <see cref="TypeGeneratorSettings" /> and stores in the project.</summary>
		/// <param name="settings">The loaded <see cref="TypeGeneratorSettings" />.</param>
		private static void CreateSettings(out TypeGeneratorSettings settings)
		{
			settings = CreateInstance<TypeGeneratorSettings>();
			AssetDatabase.CreateAsset(settings, DefaultSettingsAssetPath);
			AssetDatabase.SaveAssets();
		}

		/// <summary>Returns <see cref="SerializedObject" /> wrapped in a <see cref="SerializedObject" />.</summary>
		/// <returns><see cref="TypeGeneratorSettings" /> wrapped in a <see cref="TypeGeneratorSettings" />.</returns>
		internal static SerializedObject GetSerializedSettings()
		{
			return new SerializedObject(GetOrCreateSettings());
		}

		/// <summary>Type generation settings.</summary>
		[Serializable]
		internal sealed class Settings
		{
			/// <summary>Should this type be automatically generated.</summary>
			[SerializeField] [Tooltip("Detect changes and automatically generate file.")]
			internal bool AutoGenerate = true;

			/// <summary>The name of the type to generate.</summary>
			[SerializeField] [DelayedAttribute] [Tooltip("Name of the type to generate.")]
			internal string TypeName;

			/// <summary>The path relative to the project's asset folder.</summary>
			[SerializeField] [DelayedAttribute] [Tooltip("Location in project assets to store the generated file.")]
			internal string FilePath;

			/// <summary>Optional namespace to put the type in. Can be '<see langword="null" />' or empty..</summary>
			[Header("Optional")] [DelayedAttribute] [SerializeField] [Tooltip("Optional: Namespace for the type to reside.")]
			internal string Namespace;

			/// <summary>Backing field for <see cref="Assembly" />.</summary>
			[SerializeField] [Tooltip("Optional: If using Assembly Definitions, when checking for updated tags, which Assembly Definition to reflect on.")]
			internal AssemblyDefinitionAsset AssemblyDefinition;

			/// <summary>Used via reflection to look for the generated type.</summary>
			internal string Assembly => AssemblyDefinition == null ? "Assembly-CSharp" : AssemblyDefinition.name;
		}
	}
}
