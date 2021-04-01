using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Abstract Type Generator.</summary>
	public abstract class TypeGenerator<T> : BaseTypeGenerator, ITypeGenerator where T : TypeGenerator<T>
	{
		/// <summary>Backing field for <see cref="Generator" />.</summary>
		protected static T Instance;

		/// <summary>The instance of <see cref="TagTypeGenerator" />.</summary>
		public static ITypeGenerator Generator => Instance;

		/// <summary>Invoked when the layer file is generated.</summary>
		public event UnityAction onFileGeneration;

		/// <inheritdoc />
		public abstract void GenerateFile();

		/// <inheritdoc />
		public abstract bool CanGenerate();

		/// <summary>Invokes <see cref="onFileGeneration" />.</summary>
		protected void InvokeOnFileGeneration()
		{
			onFileGeneration?.Invoke();
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
				Debug.LogError(
					$"'{identifier}' is not a valid identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.");
				throw;
			}
		}

		/// <summary>Creates the path for the file asset.</summary>
		/// <param name="path">The path to use to create the file asset.</param>
		protected static void CreateAssetPathIfNotExists(string path)
		{
			path = path.Remove(path.LastIndexOf('/'));

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		}
	}

	/// <summary>We want to share the settings between different close constructed types.</summary>
	public abstract class BaseTypeGenerator
	{
		/// <summary>Backing field for <see cref="Settings" />.</summary>
		private static TypeGeneratorSettings settings;

		/// <summary>The <see cref="TypeGeneratorSettings" /> to use when generating files.</summary>
		protected static TypeGeneratorSettings Settings => settings ??= TypeGeneratorSettings.GetOrCreateSettings();
	}
}
