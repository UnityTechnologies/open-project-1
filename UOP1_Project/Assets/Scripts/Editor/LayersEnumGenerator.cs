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

namespace UOP1.EditorTools
{
	/// <summary>
	/// Generates a pair of <see langword="enum"/>s which contain the layer IDs and corresponding layer masks.
	/// </summary>
	internal static class LayersEnumGenerator
	{
		#region Configuration

		/// <summary>
		/// The <see cref="MenuItem" /> name.
		/// </summary>
		private const string LAYERS_MENU_ITEM = "ChopChop/Code Generation/Regenerate Layers Enums";

		/// <summary>
		/// The name of the class to enum.
		/// </summary>
		private const string LAYERS_ENUM_NAME = "Layer";

		/// <summary>
		/// The name of the Layer Masks enum to create.
		/// </summary>
		private const string LAYERMASKS_ENUM_NAME = "LayerMasks";

		/// <summary>
		/// Optional namespace to put the enums in. Can be '<see langword="null" />' or empty..
		/// </summary>
		private const string LAYERS_ENUM_NAMESPACE = "UOP1";

		/// <summary>
		/// The path relative to the project's asset folder.
		/// </summary>
		private const string LAYERS_ENUM_PATH = "Scripts/Layers.cs";

		#endregion

		#region State

		/// <summary>
		/// The absolute path to the file containing the enums.
		/// </summary>
		private static readonly string LayersFilePath = $"{Application.dataPath}/{LAYERS_ENUM_PATH}";

		/// <summary>
		/// Used to check if what layer strings and IDs are in the <see cref="Layer"/> enum.
		/// </summary>
		private static readonly HashSet<(string, int)> InEnum = new HashSet<(string, int)>();

		/// <summary>
		/// Used to check if what layer strings and IDs are in the project.
		/// </summary>
		private static readonly HashSet<(string, int)> InUnity = new HashSet<(string, int)>();

		#endregion

		/// <summary>
		/// Configures the callback for when the editor sends a message the project has changed.
		/// </summary>
		[InitializeOnLoadMethod]
		private static void ConfigureCallback()
		{
			EditorApplication.projectChanged += OnProjectChanged;
		}

		/// <summary>
		/// If the project has changed, we check if we can generate the file then check if any layers have been updated.
		/// </summary>
		private static void OnProjectChanged()
		{
			if (!CanGenerate()) return;
			if (!HasChangedLayers()) return;

			GenerateFile();
		}

		/// <summary>
		/// Checks if we can generate a new layers file.
		/// </summary>
		/// <remarks>
		/// Can only generate a new file if the following conditions are met:
		///     - <see cref="LAYERS_ENUM_NAME" /> is not null or a whitespace.
		///     - <see cref="LAYERMASKS_ENUM_NAME" /> is not null or a whitespace.
		///     - <see cref="LAYERS_ENUM_PATH" /> is not null or a whitespace.
		/// </remarks>
		/// <returns>
		/// <see langword="true" /> if all conditions are met.
		/// </returns>
		[MenuItem(LAYERS_MENU_ITEM, true)]
		private static bool CanGenerate()
		{
			if (IsNullOrWhiteSpace(LAYERS_ENUM_NAME)) return false;
			if (IsNullOrWhiteSpace(LAYERMASKS_ENUM_NAME)) return false;
			if (IsNullOrWhiteSpace(LAYERS_ENUM_PATH)) return false;

			return true;
		}

		/// <summary>
		/// Checks if the values defined in the Enum are the same as in Unity itself.
		/// </summary>
		/// <remarks>The checks are performed against the layer name and the layer ID. This should catch renames.</remarks>
		/// <returns>True if they are the <see cref="Layer"/> enum and project layers match.</returns>
		private static bool HasChangedLayers()
		{
			InUnity.Clear();

			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", Empty);
				int layerValue = LayerMask.NameToLayer(layer);

				InUnity.Add((layerName, layerValue));
			}

			InEnum.Clear();

			foreach (int enumValue in Enum.GetValues(typeof(Layer)))
				InEnum.Add((Enum.GetName(typeof(Layer), enumValue), enumValue));

			return !InEnum.SetEquals(InUnity);
		}

		/// <summary>
		/// Generates a new Layers enum file.
		/// </summary>
		[MenuItem(LAYERS_MENU_ITEM)]
		private static void GenerateFile()
		{
			// Start with a compileUnit to create our code and give it an optional namespace.
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(LAYERS_ENUM_NAMESPACE);
			compileUnit.Namespaces.Add(codeNamespace);
			codeNamespace.Imports.Add(new CodeNamespaceImport(nameof(System)));

			// Declare a pair of enum types that are public.
			CodeTypeDeclaration layersEnum = new CodeTypeDeclaration(LAYERS_ENUM_NAME)
			{
				IsEnum = true,
				TypeAttributes = TypeAttributes.Public
			};

			CodeTypeDeclaration layerMasksEnum = new CodeTypeDeclaration(LAYERMASKS_ENUM_NAME)
			{
				IsEnum = true,
				TypeAttributes = TypeAttributes.Public
			};

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
			using StringWriter stringWriter = new StringWriter();
			using CSharpCodeProvider codeProvider = new CSharpCodeProvider();

			codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, new CodeGeneratorOptions
			{
				BracingStyle = "C",
				BlankLinesBetweenMembers = false
			});

			// Create the asset path if it doesn't already exist.
			CreateAssetPathIfNotExists(LayersFilePath);

			// Write the code to the file system and refresh the AssetDatabase.
			File.WriteAllText(LayersFilePath, stringWriter.ToString());
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Adds a verbose comment on how to use the Layer enum.
		/// </summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration"/> to add the comment to.</param>
		private static void AddCommentsToLayerEnum(CodeTypeDeclaration typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\n Use this enum in place of layer names in code / scripts.\n </summary>" +
				"\n <example>\n <code>\n if (other.gameObject.layer == Layer.Characters) {\n     Destroy(other.gameObject);" +
				"\n }\n </code>\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>
		/// Adds a verbose comment on how to use the LayerMask enum.
		/// </summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration"/> to add the comment to.</param>
		private static void AddCommentsToLayerMasksEnum(CodeTypeDeclaration typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\n Use this enum in place of layer mask values in code / scripts.\n </summary>" +
				"\n <example>\n <code>\n if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), " +
				"out RaycastHit hit, Mathf.Infinity, (int) (LayerMasks.Characters | LayerMasks.Water)) {\n     " +
				"Debug.Log(\"Did Hit\");\n }\n </code>\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>
		/// Creates members for each layers in the project and adds them to the <paramref name="layersEnum"/> and <paramref name="layerMasksEnum"/>.
		/// </summary>
		/// <param name="layersEnum">The <see cref="CodeTypeDeclaration"/> to add the layer ID's to.</param>
		/// <param name="layerMasksEnum">The <see cref="CodeTypeDeclaration"/> to add the layer masks to.</param>
		private static void CreateLayerMembers(CodeTypeDeclaration layersEnum, CodeTypeDeclaration layerMasksEnum)
		{
			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", Empty);
				int layerValue = LayerMask.NameToLayer(layer);

				CodeMemberField field = new CodeMemberField(LAYERS_ENUM_NAME, layerName)
				{
					InitExpression = new CodePrimitiveExpression(layerValue)
				};
				layersEnum.Members.Add(field);

				field = new CodeMemberField(LAYERMASKS_ENUM_NAME, layerName)
				{
					InitExpression = new CodePrimitiveExpression(1 << layerValue)
				};
				layerMasksEnum.Members.Add(field);
			}
		}

		/// <summary>
		/// Creates the path for the file asset.
		/// </summary>
		/// <param name="path">The path to use to create the file asset.</param>
		private static void CreateAssetPathIfNotExists(string path)
		{
			path = path.Remove(path.LastIndexOf('/'));

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}
	}
}
