using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Scripting;

[Preserve]
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{Trigger}+{Delta}")]
public class DragComposite : InputBindingComposite<Vector2>
{
	[RuntimeInitializeOnLoadMethod]
	private static void Init() { }

	static DragComposite()
	{
		InputSystem.RegisterBindingComposite<DragComposite>();
	}

	[InputControl(layout = "Button")]
	public int Trigger;

	[InputControl(layout = "Vector2")]
	public int Delta;

	private readonly Vector2MagnitudeComparer _comparer = new Vector2MagnitudeComparer();

	public override Vector2 ReadValue(ref InputBindingCompositeContext context)
	{
		if (context.ReadValueAsButton(Trigger))
		{
			return context.ReadValue<Vector2, Vector2MagnitudeComparer>(Delta, _comparer);
		}
		return default;
	}

	public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
	{
		return ReadValue(ref context).magnitude;
	}
}
