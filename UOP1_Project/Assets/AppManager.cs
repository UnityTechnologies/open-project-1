using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class AppManager
{
  #region Singleton
  // There can be only one AppManager
  public static AppManager Instance { get; } = new AppManager();

  static AppManager()
  {
  }
  
  private AppManager()
  {
    AppInitialize();
  }
  #endregion

  private enum AppStates
  {
    None,
    Title,
    GameMenu,
    Settings,
    Play,
    Shutdown
    //todo: Game Play States, etc
  }

  private enum AppTransitions
  {
    None,
    TitleComplete,
    StartGame,
    LoadGame,
    SaveGame,
    MainMenu,
    ResumeGame,
    UpdateSettings,
    Exit,
    Confirm,
    Deny,
    Back
  }

  private UOP1.FSM35.FSM<AppStates, AppTransitions> _State = null;

  private Character.CharacterUpdate _CharacterUpdatePause = null;
  private Character.CharacterUpdate _CharacterUpdatePlay = null;
  private Character.CharacterUpdate _CharacterUpdate = null;
  public void CharacterUpdate() => _CharacterUpdate();

  private Protagonist.ProtagonistUpdate _ProtagonistUpdatePause = null;
  private Protagonist.ProtagonistUpdate _ProtagonistUpdatePlay = null;
  private Protagonist.ProtagonistUpdate _ProtagonistUpdate = null;
  public void ProtagonistUpdate() => _ProtagonistUpdate();

  // Code Execution Start point

  private void AppInitialize()
  {
    DefineStateEngine();
  }

  #region Register MonoBehaviors
  // All MonoBehaviors should register an Update method when they Awake for both Paused and Playing states
  public void AppInitializeCharacter(
    Character.CharacterUpdate characterUpdatePause,
    Character.CharacterUpdate characterUpdatePlay)
  {
    _CharacterUpdatePause = characterUpdatePause;
    _CharacterUpdatePlay = characterUpdatePlay;
    _CharacterUpdate = _CharacterUpdatePause; // default to paused update

    AppAwake();
  }

  public void AppInitializeProtagonist(
    Protagonist.ProtagonistUpdate protagonistUpdatePause,
    Protagonist.ProtagonistUpdate protagonistUpdatePlay)
  {
    _ProtagonistUpdatePause = protagonistUpdatePause;
    _ProtagonistUpdatePlay = protagonistUpdatePlay;
    _ProtagonistUpdate = _ProtagonistUpdatePause; // default to paused update

    AppAwake();
  }
  #endregion Register MonoBehaviors

  private void AppAwake() // similar to Unity Awake, call this after things are initialized
  {
    // only Awake once all MonoBehavior Updates have been registered with AppManager
    if (_CharacterUpdatePlay == null 
        || _ProtagonistUpdatePlay == null)
    {
      return;
    }
    
    // Call this after AppManager has AppInitialize<GameObject> all objects with Update() methods
    _State.Build();
    _State.Begin();
  }

  private void DefineStateEngine()
  {
    _State =
      new UOP1.FSM35.FSM<AppStates, AppTransitions>(
        AppStates.None) //Enum.GetValues(typeof(AppStates)), Enum.GetValues(typeof(AppEvents)))//, AppStates.None)
        .In(AppStates.None)
          .EntryAction(StartApp)
          .Go(AppStates.Title)

        .In(AppStates.Title)
          .EntryAction(ShowTitle)
          //.On(AppTransitions.TitleComplete).Go(AppStates.GameMenu)
          .Go(AppStates.GameMenu)
          .ExitAction(HideTitle)

        .In(AppStates.GameMenu)
          .EntryAction(ShowGameMenu)
          .Go(AppStates.Play) //todo: Remove once working Game Menu is in place
          // .On(AppTransitions.StartGame).DoAction(StartGame).Goto(AppStates.Play)
          // .On(AppTransitions.UpdateSettings).GoSub(AppStates.Settings)
          // .On(AppTransitions.LoadGame).DoAction(LoadGame)
          // .On(AppTransitions.SaveGame).DoAction(SaveGame)
          // .On(AppTransitions.Exit).Goto(AppStates.Shutdown)
          // .On(AppTransitions.ResumeGame).Goto(AppStates.Play)
          .ExitAction(HideGameMenu)

        .In(AppStates.Settings)
          .EntryAction(ShowUpdateSettings)
          .On(AppTransitions.Exit).SubReturn()
          .ExitAction(HideUpdateSettings)

        .In(AppStates.Play)
          .EntryAction(ResumePlay)
          .On(AppTransitions.UpdateSettings).GoSub(AppStates.Settings)
          .On(AppTransitions.MainMenu).Goto(AppStates.GameMenu)

        .In(AppStates.Shutdown)
          .EntryAction(ShowIsUserSureToQuit)
          .On(AppTransitions.Confirm).DoAction(ApplicationQuit)
          .On(AppTransitions.Deny).Goto(AppStates.GameMenu)
          .On(AppTransitions.Exit).Goto(AppStates.GameMenu)
          .On(AppTransitions.Back).Goto(AppStates.GameMenu);
  }

  private void StartApp()
  {
    Debug.Log($"{nameof(StartApp)}()");
    //todo
  }

  private void ShowTitle()
  {
    Debug.Log($"{nameof(ShowTitle)}()");
    //todo

    // when finished
    _State.Act(AppTransitions.TitleComplete);
  }

  private void HideTitle()
  {
    Debug.Log($"{nameof(HideTitle)}()");
    //todo
  }

  private void ShowGameMenu()
  {
    Debug.Log($"{nameof(ShowGameMenu)}()");
    //todo
    // if no game in progress, disable Resume Game
    
    _CharacterUpdate = _CharacterUpdatePause;
    _ProtagonistUpdate = _ProtagonistUpdatePause;
  }

  private void HideGameMenu()
  {
    Debug.Log($"{nameof(HideGameMenu)}()");
    //todo
  }

  private void StartGame()
  {
    Debug.Log($"{nameof(StartGame)}()");
    //todo
  }

  private void ShowUpdateSettings()
  {
    Debug.Log($"{nameof(ShowUpdateSettings)}()");
    
    _CharacterUpdate = _CharacterUpdatePause;
    _ProtagonistUpdate = _ProtagonistUpdatePause;

    //todo
  }

  private void ResumePlay()
  {
    Debug.Log($"{nameof(ResumePlay)}()");

    // MonoBehaviors resume their normal Play
    _CharacterUpdate = _CharacterUpdatePlay;
    _ProtagonistUpdate = _ProtagonistUpdatePlay;
  }

  private void HideUpdateSettings()
  {
    Debug.Log($"{nameof(HideUpdateSettings)}()");
    //todo
  }

  private void SaveGame()
  {
    Debug.Log($"{nameof(SaveGame)}()");
    //todo
  }

  private void LoadGame()
  {
    Debug.Log($"{nameof(LoadGame)}()");
    //todo
  }

  private void ShowIsUserSureToQuit()
  {
    Debug.Log($"{nameof(ShowIsUserSureToQuit)}()");
    //todo: Enable Yes/No dialog for user to pick confirm/deny
    _State.Act(AppTransitions.Confirm);
  }

  private void ApplicationQuit()
  {
    Debug.Log($"{nameof(ApplicationQuit)}()");
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
  }
}
