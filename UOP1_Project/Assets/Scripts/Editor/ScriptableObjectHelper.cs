using System;
using UnityEngine;

public static class ScriptableObjectHelper
{
	public static void GenerateButtonsForEvents<T>(UnityEngine.Object target)
		where T : ScriptableObject
	{
		var targetIr = target as T;
		if (targetIr != null)
		{
			var typeIr = targetIr.GetType();
			var events = typeIr.GetEvents();

			foreach (var ev in events)
			{
				if (GUILayout.Button(ev.Name))
				{
					//Delegates doesn't support direct access to RaiseMethod, must use backing field
					// https://stackoverflow.com/questions/14885325/eventinfo-getraisemethod-always-null
					// https://social.msdn.microsoft.com/Forums/vstudio/en-US/44b0d573-5c53-47b0-8e85-6056cbae95b0/raising-an-event-via-reflection

					var eventDelagate = typeIr.GetField(ev.Name, System.Reflection.BindingFlags.Instance |
																 System.Reflection.BindingFlags.NonPublic)
											 ?.GetValue(targetIr) as MulticastDelegate;
					try
					{
						eventDelagate?.DynamicInvoke();
					}
					catch
					{
						Debug.LogWarning($"Event '{ev.Name}' requires some arguments which weren't provided. Delegate cannot be invoked directly from UI.");
					}
				}
			}
		}
	}
}
