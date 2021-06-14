using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 51)]
public class QuestSO : ScriptableObject
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
	public bool IsDone => _isDone;
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
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this);
	}

}
