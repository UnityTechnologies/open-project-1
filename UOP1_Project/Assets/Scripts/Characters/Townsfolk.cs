using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InitialState { Idle = 0, Walk, Talk };


public class Townsfolk : MonoBehaviour
{
	public InitialState InitialState; //This is checked by conditions in the StateMachine

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
