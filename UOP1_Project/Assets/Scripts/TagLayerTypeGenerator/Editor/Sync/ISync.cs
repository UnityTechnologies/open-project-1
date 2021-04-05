using System;

namespace UOP1.TagLayerTypeGenerator.Editor.Sync
{
	/// <summary>Syncs values in a project with values in a type.</summary>
	public interface ISync
	{
		/// <summary>Are the values in project the same as the values in the type?</summary>
		/// <param name="generatingType">The type to sync with.</param>
		/// <returns>True if the values are in sync with the project.</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="generatingType" /> is null.</exception>
		bool IsInSync(Type generatingType);
	}
}
