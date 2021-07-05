using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Questline", menuName = "Quests/Questline")]
public class QuestlineSO : SerializableScriptableObject
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
	public bool IsDone
	{
		get => _isDone;
		set => _isDone = value;
	}
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
#if UNITY_EDITOR
	/// <summary>
	/// This function is only useful for the Questline Tool in Editor to remove a Questline
	/// </summary>
	/// <returns>The local path</returns>
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this);
	}
#endif

}
