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
    /// This should be visible in the inspector to see its value while debugging.
    /// </summary>
    public bool isDebugMode = false;

    /// <summary>
    /// Stores the list of all locations in the game to quickly warp to in debug mode.
    /// </summary>
    public LocationSO[] locations;

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

    // Sets isDebugMode back to false if someone checks the checkbox in the inspector by mistake.
    private void OnValidate() 
    {
        isDebugMode = false;    
    }

#endif
}
