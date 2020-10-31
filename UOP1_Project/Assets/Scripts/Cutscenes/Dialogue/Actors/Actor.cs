using UnityEngine;

/// <summary>
/// Object that hold actor data such as the name, the sprite.
/// </summary>

[CreateAssetMenu(menuName = "CutsceneSystem/Actor")]
public class Actor : ScriptableObject
{
	public string ActorName { get => _actorName; }
	public Sprite Face { get => _face; }

	[SerializeField] private string _actorName;
	[SerializeField] private Sprite _face;
}
