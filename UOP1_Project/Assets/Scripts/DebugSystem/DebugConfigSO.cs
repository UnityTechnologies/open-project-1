using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DebugConfigSO Scriptable object stores all the information reqiured while in debug mode.
/// </summary>
[CreateAssetMenu(fileName = "NewDebugConfigSO", menuName = "Debug/Debug Config")]
public class DebugConfigSO : ScriptableObject
{
    /// <summary>
    /// Stores whether it is debug mode or not
    /// This is set by <see cref="EditorColdStartup" /> script on line 37 if its an cold startup.
    /// And used by <see cref="InputReader"/> on line 163 to discard debug input if it is not debug mode.
    /// </summary>
    [HideInInspector] public bool isDebugMode = false;

    /// <summary>
    /// Stores the list of all locations in the game to quickly warp to in debug mode.
    /// </summary>
    public LocationSO[] locations;
    public int playerDebugLayerIndex = 14; // DebugNoCollision Layer
    public int playerOriginalLayerIndex = 9; // Characters Layer (Player Default Layer)

    private void OnDisable()
    {
        isDebugMode = false;
    }

#if UNITY_EDITOR

    // Sets isDebugMode to false by default
    private void Reset() 
    {
        isDebugMode = false;
    }

#endif
}
