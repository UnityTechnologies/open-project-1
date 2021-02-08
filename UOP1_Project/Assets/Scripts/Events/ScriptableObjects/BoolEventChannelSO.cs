using UnityEngine.Events;
using UnityEngine;
using System;

/// <summary>
/// This class is used for Events that have a bool argument.
/// Example: An event to toggle a UI interface
/// </summary>

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : EventChannelGenericSO <Boolean>
{
}
