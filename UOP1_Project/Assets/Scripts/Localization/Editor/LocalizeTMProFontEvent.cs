using System;
using TMPro;
using UnityEditor.Localization;
using UnityEngine.Events;

namespace UnityEngine.Localization.Components
{
	/// <summary>
	/// Component that can be used to Localize a TMP_FontAsset asset.
	/// </summary>
	[AddComponentMenu("Localization/Asset/Localize TMPro Font Event")]
	public class LocalizeTMProFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMProFont, UnityEventFont>
	{
		private void Reset()
		{
			//Set up Unity Event automatically
			var target = GetComponent<TextMeshProUGUI>();
			var setFontMethod = target.GetType().GetProperty("font").GetSetMethod();
			var methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setFontMethod) as UnityAction<TMP_FontAsset>;
			UnityEditor.Events.UnityEventTools.AddPersistentListener(this.OnUpdateAsset, methodDelegate);

			//Set up font localize asset table automatically
			var collections = LocalizationEditorSettings.GetAssetTableCollections();
			foreach (var tableCollection in collections)
			{
				if (tableCollection.name == "Fonts")
				{
					this.AssetReference.TableReference = tableCollection.TableCollectionNameReference;
					foreach (var entry in tableCollection.SharedData.Entries)
					{
						if (entry.Key == "font")
							this.AssetReference.TableEntryReference = entry.Id;
					}
				}
			}
		}
	}

	[Serializable]
	public class LocalizedTMProFont : LocalizedAsset<TMP_FontAsset> { }

	[Serializable]
	public class UnityEventFont : UnityEvent<TMP_FontAsset> { }


}
