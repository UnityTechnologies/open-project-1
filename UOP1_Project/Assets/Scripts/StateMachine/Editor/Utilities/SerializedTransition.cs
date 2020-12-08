using UnityEditor;

namespace UOP1.StateMachine.Editor
{
	internal readonly struct SerializedTransition
	{
		internal readonly SerializedProperty Transition;
		internal readonly SerializedProperty FromState;
		internal readonly SerializedProperty ToState;
		internal readonly SerializedProperty Conditions;
		internal readonly int Index;

		internal SerializedTransition(SerializedProperty transition)
		{
			Transition = transition;
			FromState = Transition.FindPropertyRelative("FromState");
			ToState = Transition.FindPropertyRelative("ToState");
			Conditions = Transition.FindPropertyRelative("Conditions");
			Index = -1;
		}

		internal SerializedTransition(SerializedObject transitionTable, int index)
		{
			Transition = transitionTable.FindProperty("_transitions").GetArrayElementAtIndex(index);
			FromState = Transition.FindPropertyRelative("FromState");
			ToState = Transition.FindPropertyRelative("ToState");
			Conditions = Transition.FindPropertyRelative("Conditions");
			Index = index;
		}

		internal SerializedTransition(SerializedProperty transition, int index)
		{
			Transition = transition.GetArrayElementAtIndex(index);
			FromState = Transition.FindPropertyRelative("FromState");
			ToState = Transition.FindPropertyRelative("ToState");
			Conditions = Transition.FindPropertyRelative("Conditions");
			Index = index;
		}

		internal void ClearProperties()
		{
			FromState.objectReferenceValue = null;
			ToState.objectReferenceValue = null;
			Conditions.ClearArray();
		}
	}
}
