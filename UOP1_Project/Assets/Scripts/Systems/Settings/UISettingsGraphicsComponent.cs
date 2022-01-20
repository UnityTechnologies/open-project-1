using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class ShadowDistanceTier
{
	public float Distance;
	public string TierDescription;
}

public class UISettingsGraphicsComponent : MonoBehaviour
{
	[FormerlySerializedAs("ShadowDistanceTierList")]
	[SerializeField] private List<ShadowDistanceTier> _shadowDistanceTierList = new List<ShadowDistanceTier>(); // filled from inspector
	[FormerlySerializedAs("URPAsset")]
	[SerializeField] private UniversalRenderPipelineAsset _uRPAsset = default;

	private int _savedResolutionIndex = default;
	private int _savedAntiAliasingIndex = default;
	private int _savedShadowDistanceTier = default;
	private bool _savedFullscreenState = default;

	private int _currentResolutionIndex = default;
	private List<Resolution> _resolutionsList = default;
	[SerializeField] UISettingItemFiller _resolutionsField = default;

	/*	private int _currentShadowQualityIndex = default;
		private List<string> _shadowQualityList = default;
		[SerializeField] private UISettingItemFiller _shadowQualityField = default;*/

	private int _currentAntiAliasingIndex = default;
	private List<string> _currentAntiAliasingList = default;
	[SerializeField] private UISettingItemFiller _antiAliasingField = default;

	private int _currentShadowDistanceTier = default;
	[SerializeField] private UISettingItemFiller _shadowDistanceField = default;
	private bool _isFullscreen = default;

	[SerializeField] private UISettingItemFiller _fullscreenField = default;

	public event UnityAction<int, int, float, bool> _save = delegate { };

	private Resolution _currentResolution;

	[SerializeField] private UIGenericButton _saveButton;
	[SerializeField] private UIGenericButton _resetButton;

	void OnEnable()
	{
		_resolutionsField.OnNextOption += NextResolution;
		_resolutionsField.OnPreviousOption += PreviousResolution;

		_shadowDistanceField.OnNextOption += NextShadowDistanceTier;
		_shadowDistanceField.OnPreviousOption += PreviousShadowDistanceTier;

		_fullscreenField.OnNextOption += NextFullscreenState;
		_fullscreenField.OnPreviousOption += PreviousFullscreenState;

		_antiAliasingField.OnNextOption += NextAntiAliasingTier;
		_antiAliasingField.OnPreviousOption += PreviousAntiAliasingTier;

		_saveButton.Clicked += SaveSettings;
		_resetButton.Clicked += ResetSettings;

	}
	private void OnDisable()
	{
		ResetSettings();
		
		_resolutionsField.OnNextOption -= NextResolution;
		_resolutionsField.OnPreviousOption -= PreviousResolution;

		_shadowDistanceField.OnNextOption -= NextShadowDistanceTier;
		_shadowDistanceField.OnPreviousOption -= PreviousShadowDistanceTier;

		_fullscreenField.OnNextOption -= NextFullscreenState;
		_fullscreenField.OnPreviousOption -= PreviousFullscreenState;

		_antiAliasingField.OnNextOption -= NextAntiAliasingTier;
		_antiAliasingField.OnPreviousOption -= PreviousAntiAliasingTier;

		_saveButton.Clicked -= SaveSettings;
		_resetButton.Clicked -= ResetSettings;
	}

	public void Init()
	{
		_resolutionsList = GetResolutionsList();
		_currentShadowDistanceTier = GetCurrentShadowDistanceTier();
		_currentAntiAliasingList = GetDropdownData(Enum.GetNames(typeof(MsaaQuality)));

		_currentResolution = Screen.currentResolution;
		_currentResolutionIndex = GetCurrentResolutionIndex();
		_isFullscreen = GetCurrentFullscreenState();
		_currentAntiAliasingIndex = GetCurrentAntialiasing();

		_savedResolutionIndex = _currentResolutionIndex;
		_savedAntiAliasingIndex = _currentAntiAliasingIndex;
		_savedShadowDistanceTier = _currentShadowDistanceTier;
		_savedFullscreenState = _isFullscreen;
	}
	
	public void Setup()
	{
		Init();
		SetResolutionField();
		SetShadowDistance();
		SetFullscreen();
		SetAntiAliasingField();
	}

	#region Resolution
	void SetResolutionField()
	{
		string displayText = _resolutionsList[_currentResolutionIndex].ToString();
		displayText = displayText.Substring(0, displayText.IndexOf("@"));

		_resolutionsField.FillSettingField(_resolutionsList.Count, _currentResolutionIndex, displayText);

	}
	List<Resolution> GetResolutionsList()
	{
		List<Resolution> options = new List<Resolution>();
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			options.Add(Screen.resolutions[i]);
		}

		return options;
	}
	int GetCurrentResolutionIndex()
	{
		if (_resolutionsList == null)
		{ _resolutionsList = GetResolutionsList(); }
		int index = _resolutionsList.FindIndex(o => o.width == _currentResolution.width && o.height == _currentResolution.height);
		return index;
	}
	void NextResolution()
	{
		_currentResolutionIndex++;
		_currentResolutionIndex = Mathf.Clamp(_currentResolutionIndex, 0, _resolutionsList.Count - 1);
		OnResolutionChange();
	}
	void PreviousResolution()
	{
		_currentResolutionIndex--;
		_currentResolutionIndex = Mathf.Clamp(_currentResolutionIndex, 0, _resolutionsList.Count - 1);
		OnResolutionChange();
	}
	void OnResolutionChange()
	{
		_currentResolution = _resolutionsList[_currentResolutionIndex];
		Screen.SetResolution(_currentResolution.width, _currentResolution.height, _isFullscreen);
		SetResolutionField();
	}
	#endregion

	#region ShadowDistance
	void SetShadowDistance()
	{
		_shadowDistanceField.FillSettingField_Localized(_shadowDistanceTierList.Count, _currentShadowDistanceTier, _shadowDistanceTierList[_currentShadowDistanceTier].TierDescription);
	}
	int GetCurrentShadowDistanceTier()
	{
		int tierIndex = -1;
		if (_shadowDistanceTierList.Exists(o => o.Distance == _uRPAsset.shadowDistance))
			tierIndex = _shadowDistanceTierList.FindIndex(o => o.Distance == _uRPAsset.shadowDistance);
		else
		{
			Debug.LogError("Current shadow distance is not in the tier List " + _uRPAsset.shadowDistance);
		}
		return tierIndex;

	}
	void NextShadowDistanceTier()
	{
		_currentShadowDistanceTier++;
		_currentShadowDistanceTier = Mathf.Clamp(_currentShadowDistanceTier, 0, _shadowDistanceTierList.Count);
		OnShadowDistanceChange();
	}
	void PreviousShadowDistanceTier()
	{
		_currentShadowDistanceTier--;
		_currentShadowDistanceTier = Mathf.Clamp(_currentShadowDistanceTier, 0, _shadowDistanceTierList.Count);
		OnShadowDistanceChange();
	}

	void OnShadowDistanceChange()
	{
		_uRPAsset.shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
		SetShadowDistance();

	}
	#endregion

	#region fullscreen
	void SetFullscreen()
	{
		if (_isFullscreen)
		{
			_fullscreenField.FillSettingField_Localized(2, 1, "On");
		}
		else
		{
			_fullscreenField.FillSettingField_Localized(2, 0, "Off");
		}

	}
	bool GetCurrentFullscreenState()
	{
		return Screen.fullScreen;
	}
	void NextFullscreenState()
	{
		_isFullscreen = true;
		OnFullscreenChange();
	}
	void PreviousFullscreenState()
	{
		_isFullscreen = false;
		OnFullscreenChange();
	}
	void OnFullscreenChange()
	{
		Screen.fullScreen = _isFullscreen;
		SetFullscreen();
	}
	#endregion

	#region Anti Aliasing
	void SetAntiAliasingField()
	{
		string optionDisplay = _currentAntiAliasingList[_currentAntiAliasingIndex].Replace("_", "");
		_antiAliasingField.FillSettingField(_currentAntiAliasingList.Count, _currentAntiAliasingIndex, optionDisplay);

	}
	int GetCurrentAntialiasing()
	{
		return _uRPAsset.msaaSampleCount;

	}
	void NextAntiAliasingTier()
	{
		_currentAntiAliasingIndex++;
		_currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
		OnAntiAliasingChange();
	}
	void PreviousAntiAliasingTier()
	{
		_currentAntiAliasingIndex--;
		_currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
		OnAntiAliasingChange();
	}

	void OnAntiAliasingChange()
	{
		_uRPAsset.msaaSampleCount = _currentAntiAliasingIndex;
		SetAntiAliasingField();

	}
	#endregion

	private List<string> GetDropdownData(string[] optionNames, params string[] customOptions)
	{
		List<string> options = new List<string>();
		foreach (string option in optionNames)
		{
			options.Add(option);
		}

		foreach (string option in customOptions)
		{
			options.Add(option);
		}
		return options;
	}

	public void SaveSettings()
	{

		_savedResolutionIndex = _currentResolutionIndex;
		_savedAntiAliasingIndex = _currentAntiAliasingIndex;
		_savedShadowDistanceTier = _currentShadowDistanceTier;
		_savedFullscreenState = _isFullscreen;
		float shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
		_save.Invoke(_currentResolutionIndex, _currentAntiAliasingIndex, shadowDistance, _isFullscreen);
	}
	
	public void ResetSettings()
	{
		_currentResolutionIndex = _savedResolutionIndex;
		OnResolutionChange();
		_currentAntiAliasingIndex = _savedAntiAliasingIndex;
		OnAntiAliasingChange();
		_currentShadowDistanceTier = _savedShadowDistanceTier;
		OnShadowDistanceChange();
		_isFullscreen = _savedFullscreenState;
		OnFullscreenChange();
	}



}
