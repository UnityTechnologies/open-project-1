using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
public class QuestSO : SerializableScriptableObject
{
	[SerializeField]
	private int _idQuest = 0;
	[Tooltip("The collection of Steps composing the Quest")]
	[SerializeField]
	private List<StepSO> _steps = new List<StepSO>();
	[SerializeField]
	bool _isDone = false;
	[SerializeField]
	VoidEventChannelSO _endQuestEvent = default;

	public int IdQuest => _idQuest;
	public List<StepSO> Steps => _steps;
	public bool IsDone
	{
		get => _isDone;
		set => _isDone = value;
	}
	public VoidEventChannelSO EndQuestEvent => _endQuestEvent; 
	public void FinishQuest()
	{
		_isDone = true;
		if(_endQuestEvent != null)
		{
			_endQuestEvent.RaiseEvent(); 
		}
	}

	public void SetQuestId(int id)
	{
		_idQuest = id;

	}
#if UNITY_EDITOR
	/// <summary>
	/// This function is only useful for the Questline Tool in Editor to remove a Quest
	/// </summary>
	/// <returns>The local path</returns>
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this);
	}
#endif

}
