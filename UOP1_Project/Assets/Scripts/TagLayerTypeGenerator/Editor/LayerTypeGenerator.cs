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
	/// <summary>Generates a file containing a type; which contains constant int definitions for each Layer in the project.</summary>
	public sealed class LayerTypeGenerator : TypeGenerator<LayerTypeGenerator>
	{
		/// <summary>Used to check if what layer strings and IDs are in the Layer type.</summary>
		private readonly HashSet<(string, int)> _inType = new HashSet<(string, int)>();

		/// <summary>Used to check if what layer strings and IDs are in the project.</summary>
		private readonly HashSet<(string, int)> _inUnity = new HashSet<(string, int)>();

		/// <summary>The absolute path to the file containing the type.</summary>
		private static string LayerFilePath => $"{Application.dataPath}/{Settings.Layer.FilePath}";

		/// <summary>Used to read the values from the type. If we don't use reflection to find the type, we tie ourselves to a specific configuration which isn't ideal.</summary>
		private static Type LayerType => Type.GetType($"{Settings.Layer.Namespace}.{Settings.Layer.TypeName}, {Settings.Layer.Assembly}");

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
			if (!Settings.Layer.AutoGenerate || !CanGenerate()) return;
			if (File.Exists(LayerFilePath) && TypeExists() && !HasChangedLayers()) return;

			GenerateFile();
		}

		/// <summary>Checks if the type exists. This will let us know if we can use reflection on it to check for changes in layers.</summary>
		/// <returns>True if the type exists.</returns>
		private bool TypeExists()
		{
			if (LayerType != null) return true;

			if (File.Exists(LayerFilePath))
				Debug.LogWarning(
					$"{Settings.Layer.Namespace}.{Settings.Layer.TypeName} is missing from {Settings.Layer.Assembly}. " +
					$"Check correct {nameof(Settings.Layer.AssemblyDefinition)} is set then regenerate via the Project Settings' menu.", Settings);

			return false;
		}

		/// <summary>Checks if we can generate a new layers file.</summary>
		/// <returns><see langword="true" /> if all conditions are met.</returns>
		public override bool CanGenerate()
		{
			if (!Settings.Layer.IsValidTypeName()) return false;
			if (!Settings.Layer.IsValidNamespace()) return false;
			if (!Settings.Layer.IsValidFilePath()) return false;

			return true;
		}

		/// <summary>Checks if the values defined in the type are the same as in Unity itself.</summary>
		/// <remarks>The checks are performed against the layer name and the layer ID. This should catch renames.</remarks>
		/// <returns>True if they the <see cref="LayerType" /> type and project layers match.</returns>
		private bool HasChangedLayers()
		{
			_inUnity.Clear();

			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", Empty);
				_inUnity.Add((layerName, LayerMask.NameToLayer(layer)));
			}

			_inType.Clear();

			var fields = LayerType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields)
				if (fieldInfo.IsLiteral)
					_inType.Add((fieldInfo.Name, (int)fieldInfo.GetValue(null)));

			return !_inType.SetEquals(_inUnity);
		}

		/// <inheritdoc />
		public override void GenerateFile()
		{
			// Start with a compileUnit to create our code and give it an optional namespace.
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(Settings.Layer.Namespace);
			compileUnit.Namespaces.Add(codeNamespace);

			// Validate the namespace.
			ValidateIdentifier(codeNamespace, Settings.Layer.Namespace);

			// Declare a type that is public and sealed.
			CodeTypeDeclaration layerType = new CodeTypeDeclaration(Settings.Layer.TypeName) {IsClass = true, TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed};
			ValidateIdentifier(layerType, Settings.Layer.TypeName);

			// Add the type declarations to the namespace.
			codeNamespace.Types.Add(layerType);

			// Add some comments so the type describes it's intended usage.
			AddCommentsToLayerType(layerType);

			// Add layer members to the type.
			CreateLayerMembers(layerType);

			// With a StringWriter and a CSharpCodeProvider; generate the code.
			using (StringWriter stringWriter = new StringWriter())
			{
				using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
				{
					codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, new CodeGeneratorOptions {BracingStyle = "C", BlankLinesBetweenMembers = false});
				}

				// Create the asset path if it doesn't already exist.
				CreateAssetPathIfNotExists(LayerFilePath);

				// Write the code to the file system and refresh the AssetDatabase.
				File.WriteAllText(LayerFilePath, stringWriter.ToString());
			}

			AssetDatabase.Refresh();

			InvokeOnFileGeneration();
		}

		/// <summary>Adds a verbose comment on how to use the Layer enum.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddCommentsToLayerType(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use this type in place of layer names in code / scripts.\r\n </summary>" +
				$"\r\n <example>\r\n <code>\r\n if (other.gameObject.layer == {Settings.Layer.TypeName}.Characters) {{\r\n     Destroy(other.gameObject);" +
				"\r\n }\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>Adds a verbose comment on how to use the Layer.Mask type.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddCommentsToLayerMaskType(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use this type in place of layer or layer mask values in code / scripts.\r\n </summary>\r\n <example>\r\n <code>\r\n if " +
				"(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, " +
				$"{Settings.Layer.TypeName}.Mask.Characters | {Settings.Layer.TypeName}.Mask.Water) {{\r\n     Debug.Log(\"Did Hit\");\r\n }}\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>Creates members for each layer in the project and adds them to the <paramref name="layerType" /> along with a nested type called "Mask".</summary>
		/// <param name="layerType">The <see cref="CodeTypeDeclaration" /> to add the layer ID's to.</param>
		private void CreateLayerMembers(CodeTypeDeclaration layerType)
		{
			// Declare a nested type for the masks.
			CodeTypeDeclaration maskType = new CodeTypeDeclaration("Mask") {IsClass = true, TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed};
			layerType.Members.Add(maskType);

			// Add comments to the nested type.
			AddCommentsToLayerMaskType(maskType);

			foreach (string layer in InternalEditorUtility.layers)
			{
				if (LayerMask.NameToLayer(layer) == 31)
					throw new InvalidOperationException("Layer 31 is used internally by the Editor’s Preview window mechanics. To prevent clashes, do not use this layer.");

				string safeName = layer.Replace(" ", Empty);

				const MemberAttributes attributes = MemberAttributes.Public | MemberAttributes.Const;

				// Layer
				CodeMemberField layerField = new CodeMemberField(typeof(int), safeName)
					{Attributes = attributes, InitExpression = new CodePrimitiveExpression(LayerMask.NameToLayer(layer))};
				ValidateIdentifier(layerField, layer);

				layerType.Members.Add(layerField);

				// Mask
				CodeMemberField maskField = new CodeMemberField(typeof(int), safeName)
					{Attributes = attributes, InitExpression = new CodePrimitiveExpression(LayerMask.GetMask(layer))};
				ValidateIdentifier(maskField, layer);

				maskType.Members.Add(maskField);
			}
		}
	}
}
