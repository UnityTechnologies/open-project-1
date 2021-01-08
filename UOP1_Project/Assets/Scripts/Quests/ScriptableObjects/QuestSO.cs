using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 51)]
public class QuestSO : ScriptableObject
{
	[Tooltip("The collection of Tasks composing the Quest")]
	[SerializeField]
	private List<TaskSO> _tasks = new List<TaskSO>();
	bool _isDone = false;
	public List<TaskSO> Tasks => _tasks;
	public bool IsDone => _isDone;
	public void FinishQuest()
	{
		_isDone = true;
	}


}
