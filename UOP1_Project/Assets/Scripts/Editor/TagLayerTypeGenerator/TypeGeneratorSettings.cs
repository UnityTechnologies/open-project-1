using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;
using static System.String;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Settings for <see cref="TagTypeGenerator" />.</summary>
	public sealed class TypeGeneratorSettings : ScriptableObject
	{
		/// <summary>Default value for <see cref="tag" /> <see cref="Settings.typeName" />.</summary>
		private const string DEFAULT_TAG_TYPE_NAME = "Tag";

		/// <summary>Default value for <see cref="layer" /> <see cref="Settings.typeName" />.</summary>
		private const string DEFAULT_LAYER_TYPE_NAME = "Layer";

		/// <summary>Default value for <see cref="tag" /> <see cref="Settings.filePath" />.</summary>
		private const string DEFAULT_TAG_FILE_PATH = "Scripts/Tag.cs";

		/// <summary>Default value for <see cref="layer" /> <see cref="Settings.filePath" />.</summary>
		private const string DEFAULT_LAYER_FILE_PATH = "Scripts/Layer.cs";

		/// <summary>Where to create a new <see cref="TypeGeneratorSettings" /> asset.</summary>
		private const string DEFAULT_SETTINGS_ASSET_PATH = "Assets/TypeGenerationSettings.asset";

		/// <summary>Where to start the asset search for settings.</summary>
		private static readonly string[] SearchInFolders = {"Assets"};

		/// <summary>Settings for Tags.</summary>
		[SerializeField] internal Settings tag;

		/// <summary>Settings for Layers.</summary>
		[SerializeField] internal Settings layer;

		/// <summary>Reset to default values.</summary>
		private void Reset()
		{
			tag = new Settings
			{
				@namespace = Application.productName.Replace(" ", Empty),
				typeName = DEFAULT_TAG_TYPE_NAME,
				filePath = DEFAULT_TAG_FILE_PATH,
				assemblyDefinition = null
			};

			layer = new Settings
			{
				@namespace = Application.productName.Replace(" ", Empty),
				typeName = DEFAULT_LAYER_TYPE_NAME,
				filePath = DEFAULT_LAYER_FILE_PATH,
				assemblyDefinition = null
			};
		}

		/// <summary>
		///     This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor
		///     only).
		/// </summary>
		private void OnValidate()
		{
			Assert.IsNotNull(tag);
			Assert.IsNotNull(layer);
			Assert.IsTrue(!IsNullOrWhiteSpace(tag.typeName));
			Assert.IsTrue(!IsNullOrWhiteSpace(tag.filePath));
			Assert.IsTrue(!IsNullOrWhiteSpace(layer.typeName));
			Assert.IsTrue(!IsNullOrWhiteSpace(layer.filePath));
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
						$"There MUST be only one {nameof(TypeGeneratorSettings)} asset in '{Application.productName}'.\n " +
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
			AssetDatabase.CreateAsset(settings, DEFAULT_SETTINGS_ASSET_PATH);
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
			internal bool autoGenerate = true;

			/// <summary>The name of the type to generate.</summary>
			[SerializeField] [Tooltip("Name of the type to generate.")]
			internal string typeName;

			/// <summary>The path relative to the project's asset folder.</summary>
			[SerializeField] [Tooltip("Location in project assets to store the generated file.")]
			internal string filePath;

			/// <summary>Optional namespace to put the type in. Can be '<see langword="null" />' or empty..</summary>
			[Header("Optional")] [SerializeField] [Tooltip("Optional: Namespace for the type to reside.")]
			internal string @namespace;

			/// <summary>Backing field for <see cref="Assembly" />.</summary>
			[SerializeField]
			[Tooltip(
				"Optional: If using Assembly Definitions, when checking for updated tags, which Assembly Definition to search in.")]
			internal AssemblyDefinitionAsset assemblyDefinition;

			/// <summary>Used via reflection to look for the generated type.</summary>
			internal string Assembly => assemblyDefinition == null ? "Assembly-CSharp" : assemblyDefinition.name;
		}
	}
}
