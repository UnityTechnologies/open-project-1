using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script needs to be put on the actor, and takes care of the current task to accomplish.
//the task contains a dialogue and maybe an event.

public class TaskManager : MonoBehaviour
{
	public List<DialogueLineSO> DefaultDialogue;

	//check if character is actif. An actif character is the character concerned by the task.
	public bool hasActifTask;

	TaskSO currentTask;

	 List<DialogueLineSO> currentDialogue;

	//play default dialogue if no task
	public void PlayDefaultDialogue()
	{


	}

	//register a task
    public void RegisterTask(TaskSO task)
	{
		currentTask = task;
		hasActifTask = true;
		if (currentTask.Dialogue != null)
		{
			currentDialogue = currentTask.Dialogue;

			if(currentTask.IsInstantanious)
			{

				StartDialogue(); 

			}
		}
	}
	//start a dialogue when interaction
	//some tasks need to be instantanious. And do not need the interact button.
	//when interaction again, restart same dialogue.
	public void InteractWithCharacter()
	{
		if(hasActifTask)
		{
			AchieveTask(); 
		}
		else
		{
			PlayDefaultDialogue(); 
		}

	}

	public void AchieveTask()
	{
		


	}
	public void StartDialogue()
	{



	}
	//next line dialogue

	//End dialogue
	public void EndDialogue()
	{



	}

	//unregister a task when it ends.
	public void UnregisterTask()
	{
		currentTask = null;
		hasActifTask = false;

		currentDialogue = null; 

		
	}
}
