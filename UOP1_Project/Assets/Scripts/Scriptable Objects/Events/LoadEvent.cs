using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LoadGameEvent", menuName = "Game Event/Load")]
public class LoadEvent : ScriptableObject
{
    public UnityAction<GameScene[], bool> loadEvent;
    public void RaiseEvent(GameScene[] locationsToLoad, bool loadScreen)
    {
        if (loadEvent != null) loadEvent.Invoke(locationsToLoad, loadScreen);
    }
}