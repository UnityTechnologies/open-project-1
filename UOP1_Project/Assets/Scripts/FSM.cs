using System;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.FSM35
{
  // Finite State Manager
  //   Tracks what screen we're on -- Title, Main Menu, Settings, Playing, etc
  //   Calls proper Update functions for each MonoBehaviour in the game
  //   Defines valid State changes
  //   Will allow MonoBehaviors to invoke Transition events for the FSM
  
  // When changing states, Standard is the default
  // But GoSub will enter a state and when you are finished there,
  // SubReturn will return back to where you came from
  // For example: Settings can be GoSub into from MainMenu or Play,
  // when you SubReturn you will go back to the one you came from  
  public enum StateTransitionType
  {
    Standard,
    GoSub,
    Return
  }
  
  public class FSM<TState, TTransition>
  {
    const int INSTANT_ACTION = -1;

    Dictionary <TTransition, FSMTransition<TTransition>> fsmTransitions = null;
    FSMTransition<TTransition> buildTransition;
    Dictionary<TState, FSMState<TState>> fsmStates = null;
    FSMState<TState> buildState;
    private Queue<FSMState<TState>> subStateHistory = new Queue<FSMState<TState>>();
    FSMAction<TState, TTransition> buildAction = null;
    private Dictionary<ActionKey, FSMAction<TState, TTransition>> fsmActions = null;
    FSMState<TState> currentState;
    private List<StateAction> entryAction = null;
    private List<StateAction> exitAction = null;
    private Dictionary<ActionKey, StateAction> doActions = null;
    private static Queue<TTransition> actionQueue = new Queue<TTransition>();
    private FSMTransition<TTransition> evaluatingTransition = null;

    private TState startingState;

    public FSM(TState startingState)
    {
      if (!typeof(TState).IsEnum)
      {
        throw new ArgumentException("S must be an enumeration");
      }
      if (!typeof(TTransition).IsEnum)
      {
        throw new ArgumentException("E must be an enumeration");
      }

      this.startingState = startingState;

      fsmStates = new Dictionary<TState, FSMState<TState>>();
      fsmTransitions = new Dictionary<TTransition, FSMTransition<TTransition>>();
      fsmActions = new Dictionary<ActionKey, FSMAction<TState, TTransition>>();
      doActions = new Dictionary<ActionKey, StateAction>();

      // Cache state and transition functions
      foreach (TState value in typeof(TState).GetEnumValues())
      {
        var newState = new FSMState<TState>(value);
        fsmStates.Add(value, newState);
      }
      
      if (fsmStates.Count < 2)
      {
        throw new InvalidOperationException("You must have at least 2 states");
      }

      foreach (TTransition value in typeof(TTransition).GetEnumValues())
      {
        var newTransition = new FSMTransition<TTransition>(value);
        fsmTransitions.Add(value, newTransition);
      }

      if (fsmTransitions.Count == 0)
      {
        throw new InvalidOperationException("You must have at least 1 Transition");
      }

      entryAction = new List<StateAction>();
      exitAction = new List<StateAction>();
    }

    public void Build()
    {
      FinalizePreviousActions();

      EnterState(startingState);

      FinishAct();
    }

    public void Begin()
    {
      CheckForInstantAction();
    }

    public void FinalizePreviousActions()
    {
      if (buildState != null)
      {
        if (entryAction.Count > 0)
        {
          foreach (var action in entryAction)
            buildState.AddEntryAction(action);
        }
        entryAction.Clear();
        if (exitAction.Count > 0)
        {
          foreach (var action in exitAction)
            buildState.AddExitAction(action);
        }
        exitAction.Clear();
      }
    }

    public FSM<TState, TTransition> In(TState state)
    {
      FinalizePreviousActions();

      buildState = fsmStates[state];

      buildTransition = null;

      return this;
    }

    /// <summary>
    /// Used after specifying your .In(<State>).On(<Transition>) you are in to move to another state
    ///   .In(AppStates.GameMenu)
    ///     .On(AppEvents.Exit).Goto(AppStates.Shutdown)
    /// </summary>
    /// <param name="state"></param>
    /// <returns>FSM for chaining</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public FSM<TState, TTransition> Goto(TState state, StateTransitionType transitionType = StateTransitionType.Standard)
    {
      if (buildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (buildTransition == null) // from On
      {
        throw new InvalidOperationException("Unknown Transition Name. Use On() first.");
      }

      var newState = fsmStates[state];

      buildAction = new FSMAction<TState, TTransition>(buildState, buildTransition, newState, transitionType);
      ActionKey key = new ActionKey() { SourceStateID = buildState.ID, SourceTransitionID = buildTransition.ID };
      fsmActions.Add(key, buildAction);

      buildTransition = null;

      return this;
    }

    public FSM<TState, TTransition> GoSub(TState state)
    {
      if (!fsmStates.ContainsKey(state))
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      
      // add the Goto to the state we can GoSub to
      Goto(state, StateTransitionType.GoSub);

      // // then add the return Transition for destination state to return to this state
      // var stashState = _BuildState;
      // _BuildState = _States[stateName];
      // Goto(stashState.Name, StateTransitionType.Return);

      return this;
    }

    public FSM<TState, TTransition> SubReturn()
    {
      if (buildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (buildTransition == null) // from On
      {
        throw new InvalidOperationException("Unknown Transition Name. Use On() first.");
      }

      buildAction = new FSMAction<TState, TTransition>(buildState, buildTransition, null, StateTransitionType.Return);
      ActionKey key = new ActionKey() { SourceStateID = buildState.ID, SourceTransitionID = buildTransition.ID };
      fsmActions.Add(key, buildAction);

      buildTransition = null;

      return this;
    }

    /// <summary>
    /// Used after specifying what state you are in to immediately progress to another state
    ///         .In(<state>).Go(<state>)
    /// </summary>
    /// <param name="state"></param>
    /// <returns>FSM for chaining</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public FSM<TState, TTransition> Go(TState state)
    {
      if (buildState == null) // must be In some state
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (buildTransition != null) // cannot be after specifying a transition
      {
        throw new InvalidOperationException($"{buildTransition} already has a Transition. Use Go() instead of On().");
      }

      var newState = fsmStates[state];

      // todo: screen for duplicate keys between this and _DoActions;

      buildAction = new FSMAction<TState, TTransition>(buildState, null, newState, StateTransitionType.Standard);
      ActionKey key = new ActionKey() { SourceStateID = buildState.ID, SourceTransitionID = INSTANT_ACTION };
      fsmActions.Add(key, buildAction);

      buildTransition = null;

      return this;
    }

    public FSM<TState, TTransition> At(TState state)
    {
      if (!fsmStates.ContainsKey(state))
      {
        throw new ArgumentException(string.Format("Statename key doesn't exist: {0}", state));
      }

      buildState = fsmStates[state];

      return this;
    }

    public FSM<TState, TTransition> On(TTransition transition)
    {
      buildTransition = fsmTransitions[transition];

      return this;
    }

    public FSM<TState, TTransition> EntryAction(StateAction stateAction)
    {
      entryAction.Add(stateAction);
      return this;
    }

    /// <summary>
    /// Does not change State, call a StateAction method for the current State + Transition
    /// </summary>
    /// <param name="stateAction">method you want to call</param>
    /// <returns>FSM35</returns>
    public FSM<TState, TTransition> DoAction(StateAction stateAction)
    {
      if (buildState == null) // from In
      {
        throw new InvalidOperationException("Unknown build state. Use In() first.");
      }
      if (buildTransition == null) // from On
      {
        throw new InvalidOperationException("Unknown Transition Name. Use On() first.");
      }

      // todo: screen for duplicates between this and _Actions;

      ActionKey key = new ActionKey() { SourceStateID = buildState.ID, SourceTransitionID = buildTransition.ID };
      doActions.Add(key, stateAction);

      return this;
    }

    public FSM<TState, TTransition> ExitAction(StateAction stateAction)
    {
      exitAction.Add(stateAction);
      return this;
    }

    public void QueueAct(TTransition transition)
    {
      actionQueue.Enqueue(transition);
    }

    public void Act(TTransition transition)
    {
      if (evaluatingTransition != null)
      {
        throw new InvalidOperationException(
          $"Cannot start a new Act '{transition}' when already evaluating '{evaluatingTransition.Transition}' -- QueueAct() it instead");
      }

      var fsmTransition = fsmTransitions[transition];

      // If you specify both a DoAction and Action for an Transition, the DoAction happens first
      foreach (var key in doActions.Keys)
      {
        if (key.SourceStateID == currentState.ID && key.SourceTransitionID == fsmTransition.ID)
        {
          if (doActions.ContainsKey(key))
          {
            var doAction = doActions[key];
            doAction();
          }
        }
      }

      evaluatingTransition = fsmTransition;

      FSMAction<TState, TTransition> newTransition = null;
      foreach (var key in fsmActions.Keys)
      {
        if (key.SourceStateID == currentState.ID && key.SourceTransitionID == fsmTransition.ID)
        {
          newTransition = fsmActions[key];
          break;
        }
      }

      if (newTransition == null)
      {
        FinishAct();
        return; // Invalid Transition for the Current State
      }

      ExitState();

      TState finalState;
      switch (newTransition.StateTransitionType())
      {
        case StateTransitionType.GoSub:
          subStateHistory.Enqueue(currentState);
          finalState = newTransition.FinalState.State;
          break;
        case StateTransitionType.Return:
          finalState = subStateHistory.Dequeue().State;
          break;
        case StateTransitionType.Standard:
          finalState = newTransition.FinalState.State;
          break;
        default:
          throw new InvalidOperationException($"Unknown StateTransitionType: {newTransition.StateTransitionType()}");
      }
      
      EnterState(finalState);

      FinishAct();
    }

    private void FinishAct()
    {
      evaluatingTransition = null;

      if (actionQueue.Count > 0)
      {
        TTransition action = actionQueue.Dequeue();
        Act(action);
      }
    }

    private void ExitState()
    {
      if (currentState.ExitAction != null)
      {
        foreach (var action in currentState.ExitAction)
        {
          action();
        }
      }
    }

    private void EnterState(TState state)
    {
      var next = fsmStates[state];
      if (next.EntryAction != null)
      {
        foreach (var action in next.EntryAction)
        {
          action();
        }
      }

      currentState = next;
      Debug.LogWarning($"Current State: {currentState}");

      CheckForInstantAction();
    }

    private void CheckForInstantAction()
    {
      FSMAction<TState, TTransition> instantAction = null;
      foreach (var key in fsmActions.Keys)
      {
        if (key.SourceStateID == currentState.ID && key.SourceTransitionID == INSTANT_ACTION)
        {
          instantAction = fsmActions[key];
          break;
        }
      }

      if (instantAction == null)
        return;

      ExitState();
      EnterState(instantAction.FinalState.State);
    }
  }
}