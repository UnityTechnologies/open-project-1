using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

	[SerializeField] private QuestAncorSO _questAnchor = default;
	// Start is called before the first frame update
	void Start()
	{
		_questAnchor.StartGame();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
