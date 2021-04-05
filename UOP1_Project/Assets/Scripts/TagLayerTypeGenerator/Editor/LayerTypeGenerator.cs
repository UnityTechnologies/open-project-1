using System;
using System.CodeDom;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UOP1.TagLayerTypeGenerator.Editor.Settings;
using UOP1.TagLayerTypeGenerator.Editor.Sync;
using static System.String;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Generates a file containing a type; which contains constant int definitions for each Layer in the project.</summary>
	internal sealed class LayerTypeGenerator : TypeGenerator<LayerTypeGenerator>
	{
		/// <inheritdoc />
		private LayerTypeGenerator(TypeGeneratorSettings.Settings settings, ISync sync) : base(settings, sync)
		{
		}

		/// <summary>Runs when the Editor starts or on a domain reload.</summary>
		[InitializeOnLoadMethod]
		public static void InitializeOnLoad()
		{
			new LayerTypeGenerator(TypeGeneratorSettings.GetOrCreateSettings.Layer, new LayerSync());
		}

		/// <summary>Creates members for each layer in the project and adds them to the <paramref name="layerType" /> along with a nested type called "Mask".</summary>
		/// <param name="layerType">The <see cref="CodeTypeDeclaration" /> to add the layer ID's to.</param>
		protected override void CreateMembers(CodeTypeDeclaration layerType)
		{
			// Make a nested type for the LayerMasks
			CodeTypeDeclaration maskType = new CodeTypeDeclaration("Mask") {IsClass = true, TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed};
			layerType.Members.Add(maskType);

			foreach (string layer in InternalEditorUtility.layers)
			{
				if (LayerMask.NameToLayer(layer) == 31)
					throw new InvalidOperationException("Layer 31 is used internally by the Editor’s Preview window mechanics. To prevent clashes, do not use this layer.");

				string safeName = layer.Replace(" ", Empty);
				const MemberAttributes attributes = MemberAttributes.Public | MemberAttributes.Const;

				AddLayerField(layerType, safeName, attributes, layer);
				AddLayerMaskField(maskType, safeName, attributes, layer);
			}

			AddCommentsToLayerType(layerType);
			AddCommentsToLayerMaskType(maskType);
		}

		private static void AddLayerField(CodeTypeDeclaration layerType, string safeName, MemberAttributes attributes, string layer)
		{
			CodeMemberField layerField = new CodeMemberField(typeof(int), safeName)
				{Attributes = attributes, InitExpression = new CodePrimitiveExpression(LayerMask.NameToLayer(layer))};
			ValidateIdentifier(layerField, layer);

			layerType.Members.Add(layerField);
		}

		private static void AddLayerMaskField(CodeTypeDeclaration maskType, string safeName, MemberAttributes attributes, string layer)
		{
			CodeMemberField maskField = new CodeMemberField(typeof(int), safeName)
				{Attributes = attributes, InitExpression = new CodePrimitiveExpression(LayerMask.GetMask(layer))};
			ValidateIdentifier(maskField, layer);

			maskType.Members.Add(maskField);
		}

		/// <summary>Adds a verbose comment on how to use the Layer enum.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddCommentsToLayerType(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use this type in place of layer names in code / scripts.\r\n </summary>" +
				$"\r\n <example>\r\n <code>\r\n if (other.gameObject.layer == {Settings.TypeName}.Characters) {{\r\n     Destroy(other.gameObject);" +
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
				$"{Settings.TypeName}.Mask.Characters | {Settings.TypeName}.Mask.Water) {{\r\n     Debug.Log(\"Did Hit\");\r\n }}\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}
	}
}
