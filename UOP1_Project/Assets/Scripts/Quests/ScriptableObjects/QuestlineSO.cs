using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Questline", menuName = "Quests/Questline", order = 51)]
public class QuestlineSO : ScriptableObject
{
	[SerializeField]
	private int _idQuestLine = 0;
	[Tooltip("The collection of Quests composing the Questline")]
	[SerializeField]
	private List<QuestSO> _quests = new List<QuestSO>();
	[SerializeField]
	bool _isDone = false;
	[SerializeField]
	VoidEventChannelSO _endQuestlineEvent = default;
	public int IdQuestline => _idQuestLine;
	public List<QuestSO> Quests => _quests;

	public VoidEventChannelSO EndQuestlineEvent => _endQuestlineEvent;
	public bool IsDone => _isDone;
	public void FinishQuestline()
	{
		if(_endQuestlineEvent!=null)
		{ _endQuestlineEvent.RaiseEvent();  }
		_isDone = true;
	}
	public void SetQuestlineId(int id)
	{
		_idQuestLine = id;
	}
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this); 
	}

}
