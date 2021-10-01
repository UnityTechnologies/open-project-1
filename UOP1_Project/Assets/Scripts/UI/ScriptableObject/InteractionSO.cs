using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Interaction", menuName = "UI/Interaction")]
public class InteractionSO : ScriptableObject
{
	[Tooltip("The interaction name")]
	[SerializeField]
	private LocalizedString _interactionName = default;

	[Tooltip("The interaction icon")]
	[SerializeField]
	private Sprite _interactionIcon = default;

	[Tooltip("The Interaction Type")]
	[SerializeField]
	private InteractionType _interactionType = default;


	public Sprite InteractionIcon => _interactionIcon;
	public LocalizedString InteractionName => _interactionName;
	public InteractionType InteractionType => _interactionType;

}
