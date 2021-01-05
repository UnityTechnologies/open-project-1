using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 51)]
public class QuestSO : ScriptableObject
{
	[Tooltip("The collection of Tasks composing the Quest")]
	
	[SerializeField]
	private List<TaskSO> _tasks = new List<TaskSO>();

	public List<TaskSO> Tasks => _tasks;


}
