using System.Collections.Generic;
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

	public int IdQuest => _idQuest;
	public List<StepSO> Steps => _steps;
	public bool IsDone => _isDone;
	public void FinishQuest()
	{
		_isDone = true;
	}

	public void SetQuestId(int id)
	{
		_idQuest = id;

	}
}
