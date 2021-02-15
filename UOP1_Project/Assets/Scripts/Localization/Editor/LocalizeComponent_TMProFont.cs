using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

namespace UnityEditor.Localization.Plugins.TMPro
{
	internal static class LocalizeComponent_TMProFont
	{
		[MenuItem("CONTEXT/TextMeshProUGUI/Localize String And Font")]
		static void LocalizeTMProText(MenuCommand command)
		{
			var target = command.context as TextMeshProUGUI;
			SetupForStringLocalization(target);
			SetupForFontLocalization(target);
		}

		public static MonoBehaviour SetupForStringLocalization(TextMeshProUGUI target)
		{
			//Avoid adding the component multiple times
			if (target.GetComponent<LocalizeStringEvent>() != null)
				return null;

			var comp = Undo.AddComponent(target.gameObject, typeof(LocalizeStringEvent)) as LocalizeStringEvent;
			var setStringMethod = target.GetType().GetProperty("text").GetSetMethod();
			var methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<string>), target, setStringMethod) as UnityAction<string>;
			Events.UnityEventTools.AddPersistentListener(comp.OnUpdateString, methodDelegate);

			const int kMatchThreshold = 5;
			var foundKey = LocalizationEditorSettings.FindSimilarKey(target.text);
			if (foundKey.collection != null && foundKey.matchDistance < kMatchThreshold)
			{
				comp.StringReference.TableEntryReference = foundKey.entry.Id;
				comp.StringReference.TableReference = foundKey.collection.TableCollectionNameReference;
			}

			return null;
		}

		public static MonoBehaviour SetupForFontLocalization(TextMeshProUGUI target)
		{
			//Avoid adding the component multiple times
			if (target.GetComponent<LocalizeTMProFontEvent>() != null)
				return null;

			var comp = Undo.AddComponent(target.gameObject, typeof(LocalizeTMProFontEvent)) as LocalizeTMProFontEvent;
			var setFontMethod = target.GetType().GetProperty("font").GetSetMethod();
			var methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setFontMethod) as UnityAction<TMP_FontAsset>;
			Events.UnityEventTools.AddPersistentListener(comp.OnUpdateAsset, methodDelegate);

			//Find font table and set it up automatically
			var collections = LocalizationEditorSettings.GetAssetTableCollections();
			foreach (var tableCollection in collections)
			{
				if (tableCollection.name == "Fonts")
				{
					comp.AssetReference.TableReference = tableCollection.TableCollectionNameReference;
					foreach (var entry in tableCollection.SharedData.Entries)
					{
						if (entry.Key == "font")
							comp.AssetReference.TableEntryReference = entry.Id;
					}
				}
			}

			return null;
		}
	}
}
