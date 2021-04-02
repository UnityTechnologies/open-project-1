using UnityEngine.Events;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Public interface for a Type generator.</summary>
	public interface ITypeGenerator
	{
		/// <summary>Invoked when the a file is generated.</summary>
		event UnityAction FileGenerated;

		/// <summary>Generates a new type file.</summary>
		void GenerateFile();

		/// <summary>Are the settings correct to generate a new type file.</summary>
		/// <returns>True if all settings are correct.</returns>
		bool CanGenerate();
	}
}
