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
	/// Generates a class which contains constant string fields with the names of the tags in the project.
	/// </summary>
	internal static class TagsClassGenerator
	{
		#region Configuration

		/// <summary>
		/// The <see cref="MenuItem" /> name.
		/// </summary>
		private const string TAGS_MENU_ITEM = "ChopChop/Code Generation/Regenerate Tag Class";

		/// <summary>
		/// Optional namespace to put the class in. Can be '<see langword="null" />' or empty..
		/// </summary>
		private const string TAGS_CLASS_NAMESPACE = "UOP1";

		/// <summary>
		/// The name of the class to create.
		/// </summary>
		private const string TAGS_CLASS_NAME = "Tag";

		/// <summary>
		/// The path relative to the project's asset folder.
		/// </summary>
		private const string TAGS_CLASS_PATH = "Scripts/Tags.cs";

		/// <summary>
		/// Used via reflection to look for the generated class. Must change if moving to another assembly definition.
		/// </summary>
		private const string ASSEMBLY_NAME = "Assembly-CSharp";

		#endregion

		#region State

		/// <summary>
		/// The absolute path to the file containing the tags.
		/// </summary>
		private static readonly string TagsFilePath = $"{Application.dataPath}/{TAGS_CLASS_PATH}";

		/// <summary>
		/// Used to read the values from the Class. If we don't use reflection to find the Class, we tie ourselves to a specific
		/// configuration which isn't ideal.
		/// </summary>
		private static readonly Type ClassType = Type.GetType($"{TAGS_CLASS_NAMESPACE}.{TAGS_CLASS_NAME}, {ASSEMBLY_NAME}");

		/// <summary>
		/// Used to check what tag strings are in the <see cref="Tag"/> class.
		/// </summary>
		private static readonly HashSet<string> InClass = new HashSet<string>();

		/// <summary>
		/// Used to check what tag strings are in the project.
		/// </summary>
		private static readonly HashSet<string> InUnity = new HashSet<string>();

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
		/// If the project has changed, we check if we can generate the file then check if any tags have been updated.
		/// </summary>
		private static void OnProjectChanged()
		{
			if (!CanGenerate()) return;
			if (!ClassTypeExists()) return;
			if (!HasChangedTags()) return;

			GenerateFile();
		}

		/// <summary>
		/// Checks if the Class type exists. This will let us know if we can use reflection on it to check for changes in tags.
		/// </summary>
		/// <returns>True if the Class type exists.</returns>
		private static bool ClassTypeExists()
		{
			if (null != ClassType) return true;

			Debug.LogWarning(
				$"{TAGS_CLASS_NAMESPACE}.{TAGS_CLASS_NAME} is missing. Regenerate via the '{TAGS_MENU_ITEM}' menu item.");
			return false;
		}

		/// <summary>
		/// Checks if the values defined in the class are the same as in Unity itself.
		/// </summary>
		/// <returns>True if the tags in the project don't match the tags in the class.</returns>
		private static bool HasChangedTags()
		{
			InUnity.Clear();

			foreach (string tag in InternalEditorUtility.tags)
				InUnity.Add(tag.Replace(" ", Empty));

			InClass.Clear();

			var fields = ClassType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields)
				if (fieldInfo.IsLiteral)
					InClass.Add(fieldInfo.Name);


			return !InClass.SetEquals(InUnity);
		}

		/// <summary>
		/// Validates if we can generate a new tags file.
		/// </summary>
		/// <remarks>
		/// Can only generate a new file if the following conditions are met:
		///     - <see cref="TAGS_CLASS_NAME" /> is not null or a whitespace.
		///     - <see cref="TAGS_CLASS_PATH" /> is not null or a whitespace.
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

			return true;
		}

		/// <summary>
		/// Generates a new Tags class file.
		/// </summary>
		[MenuItem(TAGS_MENU_ITEM)]
		private static void GenerateFile()
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
				"<summary>\r\n Use these string constants when comparing tags in code / scripts.\r\n </summary>" +
				"\r\n <example>\r\n <code>\r\n if (other.gameObject.CompareTag(Tags.Player)) {\r\n     Destroy(other.gameObject);" +
				"\r\n }\r\n </code>\r\n </example>",
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

				try
				{
					CodeGenerator.ValidateIdentifiers(field);
				}
				catch (ArgumentException)
				{
					Debug.LogError(
						$"'{tag}' cannot be made into a safe identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.");
					throw;
				}

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
