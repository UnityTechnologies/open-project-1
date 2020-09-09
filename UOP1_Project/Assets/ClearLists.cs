using UnityEngine.InputSystem;
using UnityEngine;

public class ClearLists : MonoBehaviour
{
    public ScenesData scenesData;
    public bool ClearOnStart;

    void Awake()
    {
        if (ClearOnStart)
        {
            scenesData.ClearScenesLists();
        }
        
    }

    void Update()
    {
        //We can clear lists by pressing c 
        //Just for test purpose
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            scenesData.ClearScenesLists();
        }
    }
}
