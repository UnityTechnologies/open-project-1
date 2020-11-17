using UnityEngine;
using UnityEngine.Localization;
/// <summary>
/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
/// </summary>

[CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
public class ActorSO : ScriptableObject
{
	public LocalizedString ActorName { get => _actorName; }

	[SerializeField] private LocalizedString _actorName = default;
}
