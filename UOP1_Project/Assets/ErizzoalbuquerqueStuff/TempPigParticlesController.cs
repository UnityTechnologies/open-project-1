using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UOP1.StateMachine;

/// <summary>
/// A temp script for testing for controlling Pig's particles.
/// This script uses the states of the Pig's State Machine in order to decide the moment to play the each particles system.
/// </summary>
public class TempPigParticlesController : MonoBehaviour
{
	[SerializeField] StateMachine playerStateMachine;

	[SerializeField] ParticleSystem landParticle;
	[SerializeField] ParticleSystem jumpParticle;
	[SerializeField] ParticleSystem walkingParticle;

	string previousState;
	string currentState;

    // Start is called before the first frame update
    void Start()
    {
		currentState = playerStateMachine.CurrentState;
		previousState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
		previousState = currentState;
		currentState = playerStateMachine.CurrentState;

		if (currentState == "JumpAscending" && (previousState == "Walking" || previousState == "Idle"))
		{
			//print("Jumping!!!");
			jumpParticle.Play();
		}
		else if ((currentState == "Walking" || currentState == "Idle") && previousState == "JumpDescending")
		{
			//print("Landing!!!");
			landParticle.Play();
		}

		if (currentState == "Walking" && previousState != "Walking")
		{
			//print("Started Walking!!!");
			walkingParticle.Play();
		}
		else if (currentState != "Walking" && previousState == "Walking")
		{
			//print("Stopped Walking!!!");
			walkingParticle.Stop();
		}
    }
}
