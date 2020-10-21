using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
public class Location : GameScene
{
    //Settings specific to level only
    [Header("Location specific")]
    public int enemiesCount;
}
