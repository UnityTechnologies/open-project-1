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
	/// <summary>Generates a file containing a type; which contains constant string definitions for each tag in the project.</summary>
	public sealed class TagTypeGenerator : TypeGenerator<TagTypeGenerator>
	{
		/// <summary>Used to check what tag strings are in the Tag type.</summary>
		private readonly HashSet<string> _inType = new HashSet<string>();

		/// <summary>Used to check what tag strings are in the project.</summary>
		private readonly HashSet<string> _inUnity = new HashSet<string>();

		/// <summary>The absolute path to the file containing the tags.</summary>
		private static string TagFilePath => $"{Application.dataPath}/{Settings.Tag.FilePath}";

		/// <summary>Used to read the values from the type. If we don't use reflection to find the type, we tie ourselves to a specific configuration which isn't ideal.</summary>
		private static Type TagType => Type.GetType($"{Settings.Tag.Namespace}.{Settings.Tag.TypeName}, {Settings.Tag.Assembly}");

		/// <summary>Configures the callback for when the editor sends a message the project has changed.</summary>
		[InitializeOnLoadMethod]
		private static void ConfigureCallback()
		{
			Instance = new TagTypeGenerator();
			EditorApplication.projectChanged += Instance.OnProjectChanged;
		}

		/// <summary>If the project has changed, check if I can generate the file and if any tags have been updated.</summary>
		private void OnProjectChanged()
		{
			if (!Settings.Layer.AutoGenerate || !CanGenerate()) return;
			if (File.Exists(TagFilePath) && TypeExists() && !HasChangedTags()) return;

			GenerateFile();
		}

		/// <summary>Checks if the type exists. This will let us know if we can use reflection on it to check for changes in tags.</summary>
		/// <returns>True if the type exists.</returns>
		private bool TypeExists()
		{
			if (TagType != null) return true;

			if (File.Exists(TagFilePath))
				Debug.LogWarning($"{Settings.Tag.Namespace}.{Settings.Tag.TypeName} is missing from {Settings.Tag.Assembly}. " +
				                 $"Check correct {nameof(Settings.Tag.AssemblyDefinition)} is set then regenerate via the Project Settings' menu.", Settings);

			return false;
		}

		/// <summary>Checks if the values defined in the type are the same as in Unity itself.</summary>
		/// <returns>True if the tags in the project don't match the tags in the type.</returns>
		private bool HasChangedTags()
		{
			_inUnity.Clear();

			foreach (string tag in InternalEditorUtility.tags)
				_inUnity.Add(tag.Replace(" ", Empty));

			_inType.Clear();

			var fields = TagType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields)
				if (fieldInfo.IsLiteral)
					_inType.Add(fieldInfo.Name);

			return !_inType.SetEquals(_inUnity);
		}

		/// <summary>Validates if we can generate a new tags file.</summary>
		/// <returns><see langword="true" /> if all conditions are met.</returns>
		public override bool CanGenerate()
		{
			if (!Settings.Tag.IsValidTypeName()) return false;
			if (!Settings.Tag.IsValidNamespace()) return false;
			if (!Settings.Tag.IsValidFilePath()) return false;

			return true;
		}

		/// <summary>Generates a new Tags type file.</summary>
		public override void GenerateFile()
		{
			// Start with a compileUnit to create our code and give it an optional namespace.
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(Settings.Tag.Namespace);
			compileUnit.Namespaces.Add(codeNamespace);

			// Validate the namespace.
			ValidateIdentifier(codeNamespace, Settings.Tag.Namespace);

			// Declare a type that is public and sealed.
			CodeTypeDeclaration tagType = new CodeTypeDeclaration(Settings.Tag.TypeName) {IsClass = true, TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed};
			ValidateIdentifier(tagType, Settings.Tag.TypeName);

			// Add the type declaration to the namespace.
			codeNamespace.Types.Add(tagType);

			// Add some comments so the type to describes it's intended usage.
			AddComments(tagType);

			// Create members in the type for each tag in the project.
			CreateTagMembers(tagType);

			// With a StringWriter and a CSharpCodeProvider; generate the code.
			using (StringWriter stringWriter = new StringWriter())
			{
				using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
				{
					codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, new CodeGeneratorOptions {BracingStyle = "C", BlankLinesBetweenMembers = false});
				}

				// Create the asset path if it doesn't already exist.
				CreateAssetPathIfNotExists(TagFilePath);

				// Write the code to the file system and refresh the AssetDatabase.
				File.WriteAllText(TagFilePath, stringWriter.ToString());
			}

			AssetDatabase.Refresh();

			InvokeOnFileGeneration();
		}

		/// <summary>Adds a verbose comment (like this one) to the type.</summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration" /> to add the comment to.</param>
		private void AddComments(CodeTypeMember typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\r\n Use these string constants when comparing tags in code / scripts.\r\n </summary>\r\n <example>\r\n <code>\r\n if " +
				$"(other.gameObject.CompareTag({Settings.Tag.TypeName}.Player)) {{\r\n     Destroy(other.gameObject);\r\n }}\r\n </code>\r\n </example>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>Creates members for each tag in the project and adds them to the <paramref name="tagType" />.</summary>
		/// <param name="tagType">The <see cref="CodeTypeDeclaration" /> to add the tag members to.</param>
		private void CreateTagMembers(CodeTypeDeclaration tagType)
		{
			foreach (string tag in InternalEditorUtility.tags)
			{
				CodeMemberField field = new CodeMemberField(typeof(string), tag.Replace(" ", Empty))
					{Attributes = MemberAttributes.Public | MemberAttributes.Const, InitExpression = new CodePrimitiveExpression(tag)};

				ValidateIdentifier(field, tag);

				tagType.Members.Add(field);
			}
		}
	}
}
