using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaling : MonoBehaviour
{
    // Gets the necessary canvas object
    private CanvasScaler scaler;

    // Gets the display's width and height in pixel size
    private int width = Screen.width;
    private int height = Screen.height;

    private void Start()
    {
        scaler = GetComponent<CanvasScaler>();                                      // Gets the necessary component for scaling modification

        scaler.referenceResolution = new Vector2(width, height);                    // Ensures the UI is sized according to screen pixel data
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;   // Matches the UI to referenced screen size
    }
}
