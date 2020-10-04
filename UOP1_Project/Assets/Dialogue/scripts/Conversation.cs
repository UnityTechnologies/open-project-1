
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "ScriptableObjects/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public Sentence[] lines;
    public bool triggered_once =false;
    private void Awake()
    {
        triggered_once = false;
}
}
