using UnityEngine;

/// <summary>
/// Object that hold actor data such as the name, the sprite.
/// </summary>

[CreateAssetMenu(menuName = "CutsceneSystem/Actor")]
public class Actor : ScriptableObject
{
	public string ActorName { get => _actorName; }

	[SerializeField] private string _actorName;
}
