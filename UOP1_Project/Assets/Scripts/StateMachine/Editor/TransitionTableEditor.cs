using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UOP1.StateMachine.ScriptableObjects;
using static UnityEditor.EditorGUILayout;
using Object = UnityEngine.Object;

namespace UOP1.StateMachine.Editor
{
	[CustomEditor(typeof(TransitionTableSO))]
	internal class TransitionTableEditor : UnityEditor.Editor
	{
		// Property with all the transitions.
		private SerializedProperty _transitions;

		// _fromStates and _transitionsByFromStates form an Object->Transitions dictionary.
		private List<Object> _fromStates;
		private List<List<TransitionDisplayHelper>> _transitionsByFromStates;

		// _toggles for the opened states. Only one should be active at a time.
		private bool[] _toggles;

		// Helper class to add new transitions.
		private AddTransitionHelper _addTransitionHelper;

		// Editor to display the StateSO inspector.
		private UnityEditor.Editor _cachedStateEditor;
		private bool _displayStateEditor;

		private void OnEnable()
		{
			_addTransitionHelper = new AddTransitionHelper(this);
			Undo.undoRedoPerformed += ResetIfRequired;
			Reset();
		}

		private void OnDisable()
		{
			Undo.undoRedoPerformed -= ResetIfRequired;
			_addTransitionHelper?.Dispose();
		}

		/// <summary>
		/// Method to fully reset the editor. Used whenever adding, removing and reordering transitions.
		/// </summary>
		internal void Reset()
		{
			_transitions = serializedObject.FindProperty("_transitions");
			GroupByFromState();
			_toggles = new bool[_fromStates.Count];
		}

		public override void OnInspectorGUI()
		{
			if (!_displayStateEditor)
				TransitionTableGUI();
			else
				StateEditorGUI();
		}

		private void StateEditorGUI()
		{
			Separator();

			// Back button
			if (GUILayout.Button(EditorGUIUtility.IconContent("scrollleft"), GUILayout.Width(35), GUILayout.Height(20)))
				_displayStateEditor = false;


			Separator();
			EditorGUILayout.HelpBox("Edit the Actions that a State performs per frame. The order represent the order of execution.", MessageType.Info);
			Separator();

			// State name
			EditorGUILayout.LabelField(_cachedStateEditor.target.name, EditorStyles.boldLabel);
			Separator();
			_cachedStateEditor.OnInspectorGUI();
		}

		private void TransitionTableGUI()
		{
			Separator();
			EditorGUILayout.HelpBox("Click on any State's name to see the Transitions it contains, or click the Pencil/Wrench icon to see its Actions.", MessageType.Info);
			Separator();

			serializedObject.UpdateIfRequiredOrScript();

			// For each fromState
			for (int i = 0; i < _fromStates.Count; i++)
			{
				var stateRect = BeginVertical(ContentStyle.WithPaddingAndMargins);
				EditorGUI.DrawRect(stateRect, ContentStyle.LightGray);

				var transitions = _transitionsByFromStates[i];

				// State Header
				var headerRect = BeginHorizontal();
				{
					BeginVertical();
					string label = transitions[0].SerializedTransition.FromState.objectReferenceValue.name;
					if (i == 0)
						label += " (Initial State)";

					headerRect.height = EditorGUIUtility.singleLineHeight;
					GUILayoutUtility.GetRect(headerRect.width, headerRect.height);
					headerRect.x += 5;

					// Toggle
					{
						var toggleRect = headerRect;
						toggleRect.width -= 140;
						_toggles[i] = EditorGUI.BeginFoldoutHeaderGroup(toggleRect,
							foldout: _toggles[i],
							content: label,
							style: ContentStyle.StateListStyle);
					}

					Separator();
					EndVertical();

					// State Header Buttons
					{
						bool Button(Rect position, string icon) => GUI.Button(position, EditorGUIUtility.IconContent(icon));

						var buttonRect = new Rect(
						x: headerRect.width - 105,
						y: headerRect.y,
						width: 35,
						height: 20);

						// Switch to state editor
						if (Button(buttonRect, "SceneViewTools"))
						{
							if (_cachedStateEditor == null)
								_cachedStateEditor = CreateEditor(transitions[0].SerializedTransition.FromState.objectReferenceValue, typeof(StateEditor));
							else
								CreateCachedEditor(transitions[0].SerializedTransition.FromState.objectReferenceValue, typeof(StateEditor), ref _cachedStateEditor);

							_displayStateEditor = true;
							return;
						}

						buttonRect.x += 40;
						// Move state up
						if (Button(buttonRect, "scrollup"))
						{
							if (ReorderState(i, true))
								return;
						}

						buttonRect.x += 40;
						// Move state down
						if (Button(buttonRect, "scrolldown"))
						{
							if (ReorderState(i, false))
								return;
						}
					}
				}
				EndHorizontal();

				// If state is open
				if (_toggles[i])
				{
					DisableAllStateTogglesExcept(i);
					EditorGUI.BeginChangeCheck();

					stateRect.y += EditorGUIUtility.singleLineHeight * 2;
					// Display all the transitions in the state
					foreach (var transition in transitions)
					{
						if (transition.Display(ref stateRect))
							return;
						Separator();
					}
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
				}

				EndFoldoutHeaderGroup();
				EndVertical();
				//GUILayout.HorizontalSlider(0, 0, 0);
				Separator();
			}

			var rect = BeginHorizontal();
			Space(rect.width - 55);

			// Display add transition button
			_addTransitionHelper.Display(rect);

			EndHorizontal();
		}

		/// <summary>
		/// Move a state up or down
		/// </summary>
		/// <param name="index">Index of the state in _fromStates</param>
		/// <param name="up">Moving up(true) or down(true)</param>
		/// <returns>True if changes were made and returning is required. Otherwise false.</returns>
		internal bool ReorderState(int index, bool up)
		{
			if ((up && index == 0) || (!up && index == _fromStates.Count - 1))
				return false;

			// Moving a state up is easier than moving it down. So when moving a state down, we instead move the next state up.
			MoveStateUp(up ? index : index + 1);
			return true;
		}

		private void MoveStateUp(int index)
		{
			var transitions = _transitionsByFromStates[index];
			int transitionIndex = transitions[0].SerializedTransition.Index;
			int targetIndex = _transitionsByFromStates[index - 1][0].SerializedTransition.Index;
			_transitions.MoveArrayElement(transitionIndex, targetIndex);
			serializedObject.ApplyModifiedProperties();
			Reset();
		}

		/// <summary>
		/// Add a new transition. If a transition with the same from and to states is found,
		/// the conditions in the new transition are added to it.
		/// </summary>
		/// <param name="source">Source Transition</param>
		internal void AddTransition(SerializedTransition source)
		{
			SerializedTransition transition;
			if (TryGetExistingTransition(source.FromState, source.ToState, out int fromIndex, out int toIndex))
			{
				transition = _transitionsByFromStates[fromIndex][toIndex].SerializedTransition;
			}
			else
			{
				int count = _transitions.arraySize;
				_transitions.InsertArrayElementAtIndex(count);
				transition = new SerializedTransition(_transitions.GetArrayElementAtIndex(count));
				transition.ClearProperties();
				transition.FromState.objectReferenceValue = source.FromState.objectReferenceValue;
				transition.ToState.objectReferenceValue = source.ToState.objectReferenceValue;
			}

			CopyConditions(transition.Conditions, source.Conditions);

			serializedObject.ApplyModifiedProperties();
			Reset();

			_toggles[fromIndex >= 0 ? fromIndex : _toggles.Length - 1] = true;
		}

		/// <summary>
		/// Move a transition up or down
		/// </summary>
		/// <param name="serializedTransition">The transition to move</param>
		/// <param name="up">Move up(true) or down(false)</param>
		/// <returns>True if changes were made and returning is required. Otherwise false.</returns>
		internal bool ReorderTransition(SerializedTransition serializedTransition, bool up)
		{
			int targetIndex = -1;
			int fromId = serializedTransition.FromState.objectReferenceInstanceIDValue;
			SerializedTransition st;
			for (int i = 0; i < _transitions.arraySize; i++)
			{
				if (up && i >= serializedTransition.Index)
					break;

				if (!up && i <= serializedTransition.Index)
					continue;

				st = new SerializedTransition(_transitions, i);
				if (st.FromState.objectReferenceInstanceIDValue != fromId)
					continue;

				targetIndex = i;
				if (!up)
					break;
			}

			if (targetIndex == -1)
				return false;

			_transitions.MoveArrayElement(serializedTransition.Index, targetIndex);
			serializedObject.ApplyModifiedProperties();
			Reset();

			_toggles[
				_fromStates.IndexOf(
					_transitions.GetArrayElementAtIndex(targetIndex)
					.FindPropertyRelative("FromState")
					.objectReferenceValue)] = true;

			return true;
		}

		/// <summary>
		/// Remove a transition by index.
		/// </summary>
		/// <param name="index">Index of the transition in the transition table</param>
		internal void RemoveTransition(int index)
		{
			var state = _transitions.GetArrayElementAtIndex(index).FindPropertyRelative("FromState").objectReferenceValue;
			_transitions.DeleteArrayElementAtIndex(index);
			bool toggle = DoToggleAndSort(state, index);
			serializedObject.ApplyModifiedProperties();
			Reset();

			if (toggle)
			{
				int i = _fromStates.IndexOf(state);
				if (i >= 0)
					_toggles[i] = true;
			}
		}

		private bool DoToggleAndSort(Object state, int index)
		{
			bool ret = false;
			for (int i = 0; i < _transitions.arraySize; i++)
			{
				if (_transitions.GetArrayElementAtIndex(i).FindPropertyRelative("FromState").objectReferenceValue == state)
				{
					ret = true;
					if (i > index)
					{
						_transitions.MoveArrayElement(i, index);
						break;
					}
				}
			}

			return ret;
		}

		private void DisableAllStateTogglesExcept(int index)
		{
			for (int i = 0; i < _toggles.Length; i++)
				if (i != index)
					_toggles[i] = false;
		}

		private void CopyConditions(SerializedProperty copyTo, SerializedProperty copyFrom)
		{
			for (int i = 0, j = copyTo.arraySize; i < copyFrom.arraySize; i++, j++)
			{
				copyTo.InsertArrayElementAtIndex(j);
				var cond = copyTo.GetArrayElementAtIndex(j);
				var srcCond = copyFrom.GetArrayElementAtIndex(i);
				cond.FindPropertyRelative("ExpectedResult").enumValueIndex = srcCond.FindPropertyRelative("ExpectedResult").enumValueIndex;
				cond.FindPropertyRelative("Operator").enumValueIndex = srcCond.FindPropertyRelative("Operator").enumValueIndex;
				cond.FindPropertyRelative("Condition").objectReferenceValue = srcCond.FindPropertyRelative("Condition").objectReferenceValue;
			}
		}

		private bool TryGetExistingTransition(SerializedProperty from, SerializedProperty to, out int fromIndex, out int toIndex)
		{
			fromIndex = _fromStates.IndexOf(from.objectReferenceValue);
			if (fromIndex < 0)
			{
				toIndex = -1;
				return false;
			}

			toIndex = _transitionsByFromStates[fromIndex].FindIndex(
				transitionHelper => transitionHelper.SerializedTransition.ToState.objectReferenceValue == to.objectReferenceValue);

			return toIndex >= 0;
		}

		private void ResetIfRequired()
		{
			if (serializedObject.UpdateIfRequiredOrScript())
				Reset();
		}

		private void GroupByFromState()
		{
			var groupedTransitions = new Dictionary<Object, List<TransitionDisplayHelper>>();
			int count = _transitions.arraySize;
			for (int i = 0; i < count; i++)
			{
				var serializedTransition = new SerializedTransition(_transitions, i);
				if (serializedTransition.FromState.objectReferenceValue == null)
				{
					Debug.LogError("Transition with invalid state found in table " + serializedObject.targetObject.name + ", deleting...");
					_transitions.DeleteArrayElementAtIndex(i);
					serializedObject.ApplyModifiedProperties();
					Reset();
					return;
				}
				if (serializedTransition.FromState.objectReferenceValue == null)
				{
					Debug.LogError("Transition with invalid state found in table " + serializedObject.targetObject.name + ", deleting...");
					_transitions.DeleteArrayElementAtIndex(i);
					serializedObject.ApplyModifiedProperties();
					Reset();
					return;
				}

				if (!groupedTransitions.TryGetValue(serializedTransition.FromState.objectReferenceValue, out var groupedProps))
				{
					groupedProps = new List<TransitionDisplayHelper>();
					groupedTransitions.Add(serializedTransition.FromState.objectReferenceValue, groupedProps);
				}
				groupedProps.Add(new TransitionDisplayHelper(serializedTransition, this));
			}

			_fromStates = groupedTransitions.Keys.Distinct().ToList();
			_transitionsByFromStates = new List<List<TransitionDisplayHelper>>();
			foreach (var fromState in _fromStates)
				_transitionsByFromStates.Add(groupedTransitions[fromState]);
		}
	}
}
