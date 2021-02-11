using System;
using TMPro;
using UnityEngine.Events;

namespace UnityEngine.Localization.Components
{
	/// <summary>
	/// Component that can be used to Localize a TMP_FontAsset asset.
	/// </summary>
	[AddComponentMenu("Localization/Asset/Localize TMPro Font Event")]
	public class LocalizeTMProFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMProFont, UnityEventFont>
	{
	}

	[Serializable]
	public class LocalizedTMProFont : LocalizedAsset<TMP_FontAsset> { }

	[Serializable]
	public class UnityEventFont : UnityEvent<TMP_FontAsset> { }


}
