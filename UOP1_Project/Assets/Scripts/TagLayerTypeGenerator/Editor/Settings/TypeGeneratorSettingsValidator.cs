using System;
using System.CodeDom.Compiler;
using System.Linq;
using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Editor.Settings
{
	/// <summary>Methods for validating <see cref="TypeGeneratorSettings.Settings" />s.</summary>
	internal static class TypeGeneratorSettingsValidator
	{
		/// <summary>Log errors about invalidate identifiers with this string.</summary>
		internal const string InvalidIdentifier = "'{0}' is not a valid identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.";

		/// <summary>Splits <paramref name="namespace" /> by it's "." to validate each nested namespace is a validate identifier.</summary>
		/// <returns>If all parts of the <paramref name="namespace" /> are valid.</returns>
		private static bool IsValidNamespace(string @namespace)
		{
			if (!string.IsNullOrWhiteSpace(@namespace) && @namespace.Split('.').All(CodeGenerator.IsValidLanguageIndependentIdentifier)) return true;
			Debug.LogErrorFormat(InvalidIdentifier, @namespace);
			return false;
		}

		/// <summary>Validates the <paramref name="typeName" /> is a valid identifier.</summary>
		/// <returns>True if a valid identifier.</returns>
		private static bool IsValidTypeName(string typeName)
		{
			if (CodeGenerator.IsValidLanguageIndependentIdentifier(typeName)) return true;
			Debug.LogErrorFormat(InvalidIdentifier, typeName);
			return false;
		}

		/// <summary>Validates the <paramref name="filepath" /> is valid.</summary>
		/// <returns>True if a valid path.</returns>
		private static bool IsValidFilePath(string filepath)
		{
			if (!string.IsNullOrWhiteSpace(filepath) && filepath.Substring(filepath.Length - 3) == ".cs" && Uri.IsWellFormedUriString(filepath, UriKind.Relative)) return true;
			Debug.LogError($"'{filepath}' path must be a valid path relative to the Assets folder, not an empty string and must end in '.cs'.");
			return false;
		}

		/// <summary>Validates all the settings. <see cref="IsValidNamespace" />, <see cref="IsValidTypeName" /> and <see cref="IsValidFilePath" />.</summary>
		/// <param name="settings">The <see cref="TypeGeneratorSettings.Settings" /> to validate.</param>
		/// <returns>True if all settings are valid.</returns>
		internal static bool ValidateAll(TypeGeneratorSettings.Settings settings)
		{
			return IsValidNamespace(settings.Namespace) && IsValidTypeName(settings.TypeName) && IsValidFilePath(settings.FilePath);
		}
	}
}
