using System;
using UnityEngine.Localization.Metadata;
using UnityEditor;
using UnityEngine;
[Metadata(AllowedTypes = MetadataType.AllTableEntries)] // Hint to the editor to only show this type for a Locale
[Serializable]
public class ActorInfo : IMetadata
{
	[SerializeField]
	ActorID actor;
}
public class ChoiceInfo : IMetadata
{
	[SerializeField]
	ChoiceActionType choiceAction;
}
