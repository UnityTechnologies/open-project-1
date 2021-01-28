using UnityEngine;

public class Interaction
{
	public static Interaction NONE = new Interaction(InteractionType.None, null);

	public InteractionType Type;
	public GameObject InteractableObject;

	public Interaction(InteractionType type, GameObject obj)
	{
		this.Type = type;
		this.InteractableObject = obj;
	}
}
