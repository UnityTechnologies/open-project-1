
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "ScriptableObjects/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public Sentence[] lines;
    public bool triggered_once =false;
    public bool repeatable;
   

    public void OnEnable()
    {
        if (Application.isEditor)
        {
             triggered_once = false;
}
       
    }

  
}
