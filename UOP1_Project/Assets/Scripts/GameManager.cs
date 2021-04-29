using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private  QuestManagerSO _questManager = default; 
	private void Start()
	{
        StartGame(); 
	}
	// Start is called before the first frame update
	void StartGame()
    {
        _questManager.StartGame(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
