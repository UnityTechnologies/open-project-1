using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

	[SerializeField] private QuestDataSO _questData = default;
	// Start is called before the first frame update
	void Start()
	{
		_questData.StartGame();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
