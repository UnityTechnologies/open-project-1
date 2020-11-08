using UnityEngine;

/// <summary>
/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
/// </summary>

[CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
public class ActorSO : ScriptableObject
{
	public string ActorName { get => _actorName; }

	[SerializeField] private string _actorName = default;
}
