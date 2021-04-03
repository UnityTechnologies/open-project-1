using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Share share settings between different close constructed types.</summary>
	public abstract class BaseTypeGenerator
	{
		/// <summary>Backing field for <see cref="Settings" />.</summary>
		private static TypeGeneratorSettings _settings;

		/// <summary>The <see cref="TypeGeneratorSettings" /> to use when generating files.</summary>
		protected static TypeGeneratorSettings Settings => _settings ? _settings : _settings = TypeGeneratorSettings.GetOrCreateSettings();

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
				Debug.LogError($"'{identifier}' is not a valid identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.");
				throw;
			}
		}
	}
}
