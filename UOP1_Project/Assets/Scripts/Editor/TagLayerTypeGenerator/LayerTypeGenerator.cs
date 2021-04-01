using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static System.String;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Generates a pair of <see langword="enum" />s which contain the layer IDs and corresponding layer masks.</summary>
	public sealed class LayerTypeGenerator : TypeGenerator<LayerTypeGenerator>
	{
		/// <summary>Used to check if what layer strings and IDs are in the Layer Enum.</summary>
		private readonly HashSet<(string, int)> _inEnum = new HashSet<(string, int)>();

		/// <summary>Used to check if what layer strings and IDs are in the project.</summary>
		private readonly HashSet<(string, int)> _inUnity = new HashSet<(string, int)>();

		/// <summary>The absolute path to the file containing the Enums.</summary>
		private readonly string _layerFilePath = $"{Application.dataPath}/{Settings.layer.filePath}";

		/// <summary>
		///     Used to read the values from the Enum. If we don't use reflection to find the Enums, we tie ourselves to a
		///     specific configuration which isn't ideal.
		/// </summary>
		private readonly Type _layerType =
			Type.GetType($"{Settings.layer.@namespace}.{Settings.layer.typeName}, {Settings.layer.Assembly}");

		/// <summary>Type name for layer masks.</summary>
		private readonly string _maskTypeName = $"{Settings.layer.typeName}Masks";

		/// <summary>Configures the callback for when the editor sends a message the project has changed.</summary>
		[InitializeOnLoadMethod]
		private static void ConfigureCallback()
		{
			Instance = new LayerTypeGenerator();
			EditorApplication.projectChanged += Instance.OnProjectChanged;
		}

		/// <summary>If the project has changed, we check if we can generate the file then check if any layers have been updated.</summary>
		private void OnProjectChanged()
		{
			if (!Settings.layer.autoGenerate) return;
			if (!CanGenerate()) return;
			if (!TypeExists()) return;
			if (!HasChangedLayers()) return;

			GenerateFile();
		}

		/// <summary>
		///     Checks if the Enum type exists. This will let us know if we can use reflection on it to check for changes in
		///     layers.
		/// </summary>
		/// <returns>True if the Enum type exists.</returns>
		private bool TypeExists()
		{
			if (null != _layerType) return true;

			Debug.LogWarning(
				$"{Settings.layer.@namespace}.{Settings.layer.typeName} is missing from {Settings.layer.Assembly}.\n" +
				$"Check correct {nameof(Settings.layer.assemblyDefinition)} is set then regenerate via the Project Settings' menu.",
				Settings);
			return false;
		}

		/// <summary>Checks if we can generate a new layers file.</summary>
		/// <returns><see langword="true" /> if all conditions are met.</returns>
		public override bool CanGenerate()
		{
			if (IsNullOrWhiteSpace(Settings.layer.typeName)) return false;
			if (IsNullOrWhiteSpace(Settings.layer.filePath)) return false;

			return true;
		}

		/// <summary>Checks if the values defined in the Enum are the same as in Unity itself.</summary>
		/// <remarks>The checks are performed against the layer name and the layer ID. This should catch renames.</remarks>
		/// <returns>True if they are the <see cref="_layerType" /> enum and project layers match.</returns>
		private bool HasChangedLayers()
		{
			_inUnity.Clear();

			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", Empty);
				int layerValue = LayerMask.NameToLayer(layer);

				_inUnity.Add((layerName, layerValue));
			}

			_inEnum.Clear();

			foreach (int enumValue in Enum.GetValues(_layerType))
				_inEnum.Add((Enum.GetName(_layerType, enumValue), enumValue));

			return !_inEnum.SetEquals(_inUnity);
		}

		/// <inheritdoc />
		public override void GenerateFile()
		{
			// Start with a compileUnit to create our code and give it an optional namespace.
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(Settings.layer.@namespace);
			compileUnit.Namespaces.Add(codeNamespace);
			codeNamespace.Imports.Add(new CodeNamespaceImport(nameof(System)));

			// Validate the namespace.
			ValidateIdentifier(codeNamespace, Settings.layer.@namespace);

			// Declare a pair of enum types that are public.
			CodeTypeDeclaration layersEnum = new CodeTypeDeclaration(Settings.layer.typeName)
			{
				IsEnum = true,
				TypeAttributes = TypeAttributes.Public
			};

			// Validate the type name.
			ValidateIdentifier(layersEnum, Settings.layer.typeName);

			CodeTypeDeclaration layerMasksEnum = new CodeTypeDeclaration(_maskTypeName)
			{
				IsEnum = true,
				TypeAttributes = TypeAttributes.Public
			};

			// Validate the type name.
			ValidateIdentifier(layerMasksEnum, _maskTypeName);

			// Put the Flags attribute on the LayerMasks enum to allow us to check multiple layers at once.
			layerMasksEnum.CustomAttributes.Add(new CodeAttributeDeclaration(nameof(FlagsAttribute)));

			// Add some comments so the class describes it's intended usage.
			AddCommentsToLayerEnum(layersEnum);
			AddCommentsToLayerMasksEnum(layerMasksEnum);

			// Create members in both of the enums for each layer in the project.
			CreateLayerMembers(layersEnum, layerMasksEnum);

			// Add the type declarations to the namespace.
			codeNamespace.Types.Add(layersEnum);
			codeNamespace.Types.Add(layerMasksEnum);

			// With a StringWriter and a CSharpCodeProvider; generate the code.
			using (StringWriter stringWriter = new StringWriter())
			{
				using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
				{
					codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, new CodeGeneratorOptions
					{
						BracingStyle = "C",
						BlankLinesBetweenMembers = false
					});
				}

				// Create the asset path if it doesn't already exist.
				CreateAssetPathIfNotExists(_layerFilePath);

				// Write the code to the file system and refresh the AssetDatabase.
				File.WriteAllText(_layerFilePath, stringWriter.ToString());
			}

			AssetDatabase.Refresh();

			InvokeOnFileGeneration();
		}

		/// <summary>Adds a verbose comment on how to use the Layer enum.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddCommentsToLayerEnum(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use this enum in place of layer names in code / scripts.\r\n </summary>" +
				"\r\n <example>\r\n <code>\r\n if (other.gameObject.layer == Layer.Characters) {\r\n     Destroy(other.gameObject);" +
				"\r\n }\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>Adds a verbose comment on how to use the LayerMask enum.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddCommentsToLayerMasksEnum(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use this enum in place of layer mask values in code / scripts.\r\n </summary>" +
				"\r\n <example>\r\n <code>\r\n if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), " +
				"out RaycastHit hit, Mathf.Infinity, (int) (LayerMasks.Characters | LayerMasks.Water)) {\r\n     " +
				"Debug.Log(\"Did Hit\");\r\n }\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>
		///     Creates members for each layers in the project and adds them to the <paramref name="layersEnum" /> and
		///     <paramref name="layerMasksEnum" />.
		/// </summary>
		/// <param name="layersEnum">The <see cref="CodeTypeDeclaration" /> to add the layer ID's to.</param>
		/// <param name="layerMasksEnum">The <see cref="CodeTypeDeclaration" /> to add the layer masks to.</param>
		private void CreateLayerMembers(CodeTypeDeclaration layersEnum, CodeTypeDeclaration layerMasksEnum)
		{
			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", Empty);
				int layerValue = LayerMask.NameToLayer(layer);

				// Layer ID enum
				CodeMemberField field = new CodeMemberField(Settings.layer.typeName, layerName)
				{
					InitExpression = new CodePrimitiveExpression(layerValue)
				};
				ValidateIdentifier(field, layer);
				layersEnum.Members.Add(field);

				// LayerMasks enum
				field = new CodeMemberField(_maskTypeName, layerName)
				{
					InitExpression = new CodePrimitiveExpression(1 << layerValue)
				};
				ValidateIdentifier(field, layer);
				layerMasksEnum.Members.Add(field);
			}
		}
	}
}
