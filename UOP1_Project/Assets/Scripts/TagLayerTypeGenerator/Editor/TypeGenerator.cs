using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UOP1.TagLayerTypeGenerator.Editor.Settings;
using UOP1.TagLayerTypeGenerator.Editor.Sync;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Generates a file containing a type; which contains constant string definitions for each tag in the project.</summary>
	internal abstract class TypeGenerator<T> : ITypeGenerator where T : ITypeGenerator
	{
		/// <summary>Backing field for <see cref="Generator" />.</summary>
		private static TypeGenerator<T> _instance;

		/// <summary>Checks for updates to the Tags in the Project.</summary>
		private readonly ISync _sync;

		/// <summary>The settings for the <see cref="TypeGenerator{T}" />.</summary>
		protected readonly TypeGeneratorSettings.Settings Settings;

		/// <summary>Instantiates a new instance of <see cref="TypeGenerator{T}" />. Subscribes to <see cref="EditorApplication.projectChanged" />.</summary>
		/// <param name="settings">The settings to use for this <see cref="TypeGenerator{T}" />.</param>
		/// <param name="sync">Sync values between the project and the type.</param>
		protected TypeGenerator(TypeGeneratorSettings.Settings settings, ISync sync)
		{
			_instance = this;
			_sync = sync;
			Settings = settings;
			EditorApplication.projectChanged += OnProjectChanged;
		}

		/// <summary>Instance of <see cref="ITypeGenerator" />.</summary>
		public static ITypeGenerator Generator => _instance;

		/// <summary>Used to read the values from the type. If we don't use reflection to find the type, we tie ourselves to a specific configuration which isn't ideal.</summary>
		private Type GeneratingType => Type.GetType($"{Settings.Namespace}.{Settings.TypeName}, {Settings.Assembly}");

		/// <summary>The absolute path for the generated code.</summary>
		private string AbsoluteFilePath => $"{Application.dataPath}/{Settings.FilePath}";

		/// <inheritdoc />
		public event UnityAction FileGenerated;

		/// <summary>Validates if we can generate a new tags file.</summary>
		/// <returns><see langword="true" /> if all conditions are met.</returns>
		public bool CanGenerate()
		{
			return TypeGeneratorSettingsValidator.ValidateAll(Settings);
		}

		/// <summary>Generates a new Tags type file.</summary>
		public void GenerateFile()
		{
			CodeCompileUnit compileUnit = new CodeCompileUnit();

			SetupNamespaceAndType(compileUnit);

			CreateMembers(compileUnit.Namespaces[0].Types[0]);

			WriteCodeToDisk(compileUnit);

			AssetDatabase.Refresh();

			FileGenerated?.Invoke();
		}

		/// <summary>If the project has changed, check if I can generate the file and if any tags have been updated.</summary>
		private void OnProjectChanged()
		{
			if (!Settings.AutoGenerate || !CanGenerate()) return;
			if (File.Exists(AbsoluteFilePath) && TypeAlreadyExists(GeneratingType) && IsSynced()) return;

			GenerateFile();
		}

		/// <summary>Checks if the type exists. This will let us know if we can use reflection on it to check for changes in tags.</summary>
		/// <returns>True if the type exists.</returns>
		private bool TypeAlreadyExists(Type generatingType)
		{
			if (generatingType != null) return true;

			if (File.Exists(AbsoluteFilePath))
				Debug.LogWarning(
					$"{Settings.Namespace}.{Settings.TypeName} is missing from {Settings.Assembly}. " +
					$"Check correct {nameof(Settings.AssemblyDefinition)} is set then regenerate via the Project Settings' menu.",
					TypeGeneratorSettings.GetOrCreateSettings);

			return false;
		}

		/// <summary>Checks if the values defined in the type are the same as in Unity itself.</summary>
		/// <returns>True if the type members match the project values.</returns>
		private bool IsSynced()
		{
			return _sync.IsInSync(GeneratingType);
		}

		private void SetupNamespaceAndType(CodeCompileUnit codeCompileUnit)
		{
			CodeNamespace codeNamespace = new CodeNamespace(Settings.Namespace);
			ValidateIdentifier(codeNamespace, Settings.Namespace);
			codeCompileUnit.Namespaces.Add(codeNamespace);

			CodeTypeDeclaration tagType = new CodeTypeDeclaration(Settings.TypeName) {IsClass = true, TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed};
			ValidateIdentifier(tagType, Settings.TypeName);
			codeNamespace.Types.Add(tagType);
		}

		private void WriteCodeToDisk(CodeCompileUnit compileUnit)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions {BracingStyle = "C", BlankLinesBetweenMembers = false};
				using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
				{
					codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, codeGeneratorOptions);
				}

				CreateAssetPathIfNotExists(AbsoluteFilePath);

				File.WriteAllText(AbsoluteFilePath, stringWriter.ToString());
			}
		}

		/// <summary>Creates members in the <paramref name="typeDeclaration" />.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add members to.</param>
		protected abstract void CreateMembers(CodeTypeDeclaration typeDeclaration);

		/// <summary>Creates the path for the file asset.</summary>
		/// <param name="path">The path to use to create the file asset.</param>
		private static void CreateAssetPathIfNotExists(string path)
		{
			path = path.Remove(path.LastIndexOf('/'));

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		}

		/// <summary>Validates a given field.</summary>
		/// <param name="field">The member field to validate.</param>
		/// <param name="identifier">The identifier we're creating a member for.</param>
		protected static void ValidateIdentifier(CodeObject field, string identifier)
		{
			try
			{
				CodeGenerator.ValidateIdentifiers(field);
			}
			catch (ArgumentException)
			{
				Debug.LogErrorFormat(TypeGeneratorSettingsValidator.InvalidIdentifier, identifier);
				throw;
			}
		}
	}
}
