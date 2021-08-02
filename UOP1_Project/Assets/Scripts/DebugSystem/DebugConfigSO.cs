using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDebugConfigSO", menuName = "Debug/Debug Config")]
public class DebugConfigSO : ScriptableObject
{
    public LocationSO[] locations;
}
