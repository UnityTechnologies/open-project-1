using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Editor.Settings
{
	/// <summary>Holds the defaults values for the <see cref="TypeGeneratorSettings" />.</summary>
	internal static class TypeGeneratorSettingsDefaults
	{
		/// <summary>Default value for <see cref="TypeGeneratorSettings.Tag" /> <see cref="TypeGeneratorSettings.Settings.TypeName" />.</summary>
		private const string DefaultTagTypeName = "Tag";

		/// <summary>Default value for <see cref="TypeGeneratorSettings.Layer" /> <see cref="TypeGeneratorSettings.Settings.TypeName" />.</summary>
		private const string DefaultLayerTypeName = "Layer";

		/// <summary>Default value for <see cref="TypeGeneratorSettings.Tag" /> <see cref="TypeGeneratorSettings.Settings.FilePath" />.</summary>
		private const string DefaultTagFilePath = "Scripts/Tag.cs";

		/// <summary>Default value for <see cref="TypeGeneratorSettings.Layer" /> <see cref="TypeGeneratorSettings.Settings.FilePath" />.</summary>
		private const string DefaultLayerFilePath = "Scripts/Layer.cs";

		/// <summary>Where to start the asset search for settings.</summary>
		internal static readonly string[] SearchInFolders = {"Assets"};

		/// <summary>Default settings for <see cref="TypeGeneratorSettings.Tag" />.</summary>
		internal static readonly TypeGeneratorSettings.Settings Tag = new TypeGeneratorSettings.Settings
		{
			AutoGenerate = true,
			TypeName = DefaultTagTypeName,
			FilePath = DefaultTagFilePath,
			Namespace = DefaultNamespace,
			AssemblyDefinition = null
		};

		/// <summary>Default settings for <see cref="TypeGeneratorSettings.Layer" />.</summary>
		internal static readonly TypeGeneratorSettings.Settings Layer = new TypeGeneratorSettings.Settings
		{
			AutoGenerate = true,
			TypeName = DefaultLayerTypeName,
			FilePath = DefaultLayerFilePath,
			Namespace = DefaultNamespace,
			AssemblyDefinition = null
		};

		/// <summary>Default namespace for <see cref="TypeGeneratorSettings.Tag" /> and  <see cref="TypeGeneratorSettings.Layer" />.</summary>
		private static string DefaultNamespace => Application.productName.Replace(" ", string.Empty);
	}
}
