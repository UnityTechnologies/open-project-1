using System.CodeDom;
using System.CodeDom.Compiler;
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
	/// Generates a class which contains constant string fields with the names of the tags in the project.
	/// </summary>
	internal static class TagsClassGenerator
	{
		#region Configuration

		/// <summary>
		/// The <see cref="MenuItem" /> name.
		/// </summary>
		private const string TAGS_MENU_ITEM = "ChopChop/Code Generation/Tags";

		/// <summary>
		/// The name of the class to create.
		/// </summary>
		private const string TAGS_CLASS_NAME = "Tags";

		/// <summary>
		/// Optional namespace to put the class in. Can be '<see langword="null" />' or empty..
		/// </summary>
		private const string TAGS_CLASS_NAMESPACE = "UOP1";

		/// <summary>
		/// The path relative to the project's asset folder.
		/// </summary>
		private const string TAGS_CLASS_PATH = "Scripts/Tags.cs";

		#endregion

		/// <summary>
		/// The absolute path to the file containing the tags.
		/// </summary>
		private static readonly string TagsFilePath = $"{Application.dataPath}/{TAGS_CLASS_PATH}";

		/// <summary>
		/// Validates if we can generate a new tags file.
		/// </summary>
		/// <remarks>
		/// Can only generate a new file if the following conditions are met:
		///     - <see cref="TAGS_CLASS_NAME" /> is not null or a whitespace.
		///     - <see cref="TAGS_CLASS_PATH" /> is not null or a whitespace.
		///     - <see cref="TagsFilePath" /> doesn't already exist.
		/// </remarks>
		/// <remarks>
		/// These are protections against accidentally overwriting another file if the configuration values are changed.
		/// </remarks>
		/// <returns>
		/// <see langword="true" /> if all conditions are met.
		/// </returns>
		[MenuItem(TAGS_MENU_ITEM, true)]
		private static bool CanGenerate()
		{
			if (IsNullOrWhiteSpace(TAGS_CLASS_NAME)) return false;
			if (IsNullOrWhiteSpace(TAGS_CLASS_PATH)) return false;
			if (File.Exists(TagsFilePath)) return false;

			return true;
		}

		/// <summary>
		/// Generates a new Tags class file.
		/// </summary>
		[MenuItem(TAGS_MENU_ITEM)]
		private static void Generate()
		{
			// Start with a compileUnit to create our code and give it an optional namespace.
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(TAGS_CLASS_NAMESPACE);
			compileUnit.Namespaces.Add(codeNamespace);

			// Declare a type that is public and sealed.
			CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration(TAGS_CLASS_NAME)
			{
				IsClass = true,
				TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
			};

			// Add some comments so the class describes it's intended usage.
			AddComments(typeDeclaration);

			// Create members in the type for each tag in the project.
			CreateTagMembers(typeDeclaration);

			// Add the type declaration to the namespace.
			codeNamespace.Types.Add(typeDeclaration);

			// With a StringWriter and a CSharpCodeProvider; generate the code.
			using StringWriter stringWriter = new StringWriter();
			using CSharpCodeProvider codeProvider = new CSharpCodeProvider();

			codeProvider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, new CodeGeneratorOptions
			{
				BracingStyle = "C",
				BlankLinesBetweenMembers = false
			});

			// Create the asset path if it doesn't already exist.
			CreateAssetPathIfNotExists(TagsFilePath);

			// Write the code to the file system and refresh the AssetDatabase.
			File.WriteAllText(TagsFilePath, stringWriter.ToString());
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Adds a verbose comment (like this one) to the class.
		/// </summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration"/> to add the comment to.</param>
		private static void AddComments(CodeTypeDeclaration typeDeclaration)
		{
			CodeCommentStatement commentStatement = new CodeCommentStatement(
				"<summary>\n Use these string constants when comparing tags in code / scripts.\n </summary>" +
				"\n <example>\n <code>\n if (other.gameObject.CompareTag(Tags.Player)) {\n     Destroy(other.gameObject);" +
				"\n }\n </code>\n </example>\n <remarks>\n <b>Important</b>: To regenerate this class after adding or removing " +
				$"tags to the project; delete\n \"{TAGS_CLASS_PATH}\" via the \"Project Window\" and use the " +
				$"\"{TAGS_MENU_ITEM}\" menu to regenerate it.\n </remarks>",
				true);

			typeDeclaration.Comments.Add(commentStatement);
		}

		/// <summary>
		/// Creates members for each tag in the project and adds them to the <paramref name="typeDeclaration"/>.
		/// </summary>
		/// <param name="typeDeclaration">The <see cref="CodeTypeDeclaration"/> to add the tag members to.</param>
		private static void CreateTagMembers(CodeTypeDeclaration typeDeclaration)
		{
			foreach (string tag in InternalEditorUtility.tags)
			{
				CodeMemberField field = new CodeMemberField
				{
					Attributes = MemberAttributes.Public | MemberAttributes.Const,
					Name = tag.Replace(" ", Empty),
					Type = new CodeTypeReference(typeof(string)),
					InitExpression = new CodePrimitiveExpression(tag)
				};

				typeDeclaration.Members.Add(field);
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
