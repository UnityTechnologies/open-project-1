using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public LoadEvent onLevelEnd;
    public GameScene[] locationsToLoad;
    public bool showLoadScreen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onLevelEnd.RaiseEvent(locationsToLoad, showLoadScreen);
        }
    }
}
