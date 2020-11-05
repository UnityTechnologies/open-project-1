using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{

	[SerializeField] private InputReader _inputReader = default;

	private PlayableDirector _activePlayableDirector;

	private void Awake()
	{ 
			_inputReader.gameInput.Menus.Advance.performed += ctx => OnAdvance();
	}

	public void Play(PlayableDirector activePlayableDirector)
	{
		_activePlayableDirector = activePlayableDirector;

		_activePlayableDirector.Play();
		_activePlayableDirector.stopped += ctx => CutsceneEnded();

		DisableGameplayInput(); 
	}

	public void CutsceneEnded()
	{
		EnableGameplayInput();
	}

	private void OnAdvance()
	{
		
	}

	public void PauseTimeline()
	{
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
	}

	public void ResumeTimeline()
	{
		_activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
	}

	private void EnableGameplayInput()
	{
		_inputReader.gameInput.Gameplay.Enable();
		_inputReader.gameInput.Menus.Disable();
	}

	private void DisableGameplayInput()
	{
		_inputReader.gameInput.Menus.Enable();
		_inputReader.gameInput.Gameplay.Disable();
	}
}
