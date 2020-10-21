using UnityEngine;

public class StartGame : MonoBehaviour
{
    public LoadEvent onPlayButtonPress;
    public GameScene[] locationsToLoad;
    public bool showLoadScreen;

    public void OnPlayButtonPress()
    {
        onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
    }
}
