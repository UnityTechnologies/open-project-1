using System.IO;
using UnityEngine.Events;

namespace UOP1.TagLayerTypeGenerator.Editor
{
	/// <summary>Abstract Type Generator.</summary>
	public abstract class TypeGenerator<T> : BaseTypeGenerator, ITypeGenerator where T : TypeGenerator<T>
	{
		/// <summary>Backing field for <see cref="Generator" />.</summary>
		protected static T Instance;

		/// <summary>Instance of <see cref="ITypeGenerator" />.</summary>
		public static ITypeGenerator Generator => Instance;

		/// <summary>Invoked when the file is generated.</summary>
		public event UnityAction FileGenerated;

		/// <inheritdoc />
		public abstract void GenerateFile();

		/// <inheritdoc />
		public abstract bool CanGenerate();

		/// <summary>Invokes <see cref="FileGenerated" />.</summary>
		protected void InvokeOnFileGeneration()
		{
			FileGenerated?.Invoke();
		}

		/// <summary>Creates the path for the file asset.</summary>
		/// <param name="path">The path to use to create the file asset.</param>
		protected static void CreateAssetPathIfNotExists(string path)
		{
			path = path.Remove(path.LastIndexOf('/'));

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		}
	}
}
