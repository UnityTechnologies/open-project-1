using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXUtil : MonoBehaviour
{
    public static float RemapValue(float value, float low1, float high1, float low2, float high2) 
    {
        float x =  low2 + (value - low1) * (high2 - low2) / (high1 - low1);
        return x;
    }
}