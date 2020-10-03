using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public Vector2 range;

    public MinMaxAttribute(float min, float max)
    {
        range = new Vector2(min, max);
    }
}