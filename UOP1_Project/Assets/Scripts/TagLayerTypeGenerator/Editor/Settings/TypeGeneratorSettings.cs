using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static System.String;

namespace UOP1.TagLayerTypeGenerator.Editor.Settings
{
	/// <summary>Settings for <see cref="TagTypeGenerator" />.</summary>
	public sealed class TypeGeneratorSettings : ScriptableObject
	{
		/// <summary>Where to create a new <see cref="TypeGeneratorSettings" /> asset.</summary>
		private const string DefaultSettingsAssetPath = "Assets/TypeGeneratorSettings.asset";

		/// <summary>Settings for Tags.</summary>
		[SerializeField] internal Settings Tag;

		/// <summary>Settings for Layers.</summary>
		[SerializeField] internal Settings Layer;

		/// <summary>Returns <see cref="InvalidOperationException" /> or creates a new one and saves the asset.</summary>
		/// <value>The <see cref="TypeGeneratorSettings" /> to use.</value>
		/// <exception cref="TypeGeneratorSettings">More than one <see cref="TypeGeneratorSettings" /> are in the project.</exception>
		internal static TypeGeneratorSettings GetOrCreateSettings
		{
			get
			{
				string[] guids = AssetDatabase.FindAssets($"t:{nameof(TypeGeneratorSettings)}", TypeGeneratorSettingsDefaults.SearchInFolders);

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
						throw new InvalidOperationException($"There MUST be only one {nameof(TypeGeneratorSettings)} asset in '{Application.productName}'.\n " +
						                                    $"Found: {Join(", ", guids.Select(AssetDatabase.GUIDToAssetPath))}.");
				}

				return settings;
			}
		}

		/// <summary>Reset to default values.</summary>
		private void Reset()
		{
			Tag = TypeGeneratorSettingsDefaults.Tag;
			Layer = TypeGeneratorSettingsDefaults.Layer;
		}

		/// <summary>This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor only).</summary>
		private void OnValidate()
		{
			TypeGeneratorSettingsValidator.ValidateAll(Tag);
			TypeGeneratorSettingsValidator.ValidateAll(Layer);
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
			return new SerializedObject(GetOrCreateSettings);
		}

		/// <summary>Type generation settings.</summary>
		[Serializable]
		public sealed class Settings
		{
			/// <summary>When Assembly Definitions are not in use, Unity puts all scripts in this assembly.</summary>
			private const string DefaultUnityAssemblyName = "Assembly-CSharp";

			/// <summary>Should this type be automatically generated.</summary>
			[SerializeField] [Tooltip("Detect changes and automatically generate file.")]
			internal bool AutoGenerate = true;

			/// <summary>The name of the type to generate.</summary>
			[SerializeField] [Delayed] [Tooltip("Name of the type to generate.")]
			internal string TypeName;

			/// <summary>The path relative to the project's asset folder.</summary>
			[SerializeField] [Delayed] [Tooltip("Location in project assets to store the generated file.")]
			internal string FilePath;

			/// <summary>Optional namespace to put the type in. Can be '<see langword="null" />' or empty..</summary>
			[Header("Optional")] [Delayed] [SerializeField] [Tooltip("Optional: Namespace for the type to reside.")]
			internal string Namespace;

			/// <summary>Backing field for <see cref="Assembly" />.</summary>
			[SerializeField] [Tooltip("Optional: If using Assembly Definitions, when checking for updated tags, which Assembly Definition to search in.")]
			internal AssemblyDefinitionAsset AssemblyDefinition;

			/// <summary>Used via reflection to look for the generated type.</summary>
			internal string Assembly => AssemblyDefinition == null ? DefaultUnityAssemblyName : AssemblyDefinition.name;
		}
	}
}
