using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Interaction", menuName = "UI/Interaction", order = 51)]
public class InteractionSO : ScriptableObject
{
	[Tooltip("The interaction name")]
	[SerializeField]
	private LocalizedString _interactionName = default;

	[Tooltip("The Interaction Type")]
	[SerializeField]
	private InteractionType _interactionType = default;



	public LocalizedString InteractionName => _interactionName;
	public InteractionType InteractionType => _interactionType;

}
