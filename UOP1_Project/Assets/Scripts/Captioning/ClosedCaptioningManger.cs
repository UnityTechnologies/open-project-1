using Assets.Scripts.Captioning.CaptionEmitters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class ClosedCaptioningManger : MonoBehaviour
	{
		[Header("SoundEmitters pool")]
		[SerializeField] private CaptionEmitterFactorySO _factory = default;
		[SerializeField] private CaptionEmitterPoolSO _pool = default;
		[SerializeField] private int _initialSize = 10;

		[Header("Listening on channels")]
		[Tooltip("The ClosedCaptioningManger listens to this event, fired by objects in any scene, to display text for the SFXs")]
		[SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;
		[Tooltip("The ClosedCaptioningManger listens to this event, fired by settings menu or SettingsSystem initializator, to turn on/off captioning")]
		[SerializeField] private BoolEventChannelSO _changeCaptioningEventChannel = default;

		[Header("Captions display settings")]
		[Tooltip("Settings object")]
		[SerializeField] private SettingsSO _currentSettings = default;
		[Tooltip("Space between captions if many available for same source")]
		[SerializeField] private float _spaceBetweenCaptions = 0.5f;

		private bool _isCaptioningEnabled => _currentSettings.IsCaptioningEnabled;
		private List<CaptionEmitter> captionEmmiters = new List<CaptionEmitter>();
		private UnityEngine.Camera MainCamera;		

		private void Awake()
		{
			_pool.Prewarm(_initialSize);
			_pool.SetParent(this.transform);
		}

		private void OnEnable()
		{
			_SFXEventChannel.OnAudioCuePlayRequested += DisplayCaption;
		}

		private void OnDestroy()
		{
			_SFXEventChannel.OnAudioCuePlayRequested -= DisplayCaption;
		}

		private void Update()
		{
			if (_isCaptioningEnabled && captionEmmiters.Count > 0)
			{
				for (int i = 0; i < captionEmmiters.Count; i++)
				{
					captionEmmiters[i].GetOffscreentTargetIndicator().UpdateTargetIndicator();
				}
			}
		}

		public AudioCueKey DisplayCaption(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
		{
			if (_isCaptioningEnabled)
			{
				VisualisableAudioClip[] clipsToPlay = audioCue.GetClips();
				CaptionEmitter[] captionEmitterArray = new CaptionEmitter[clipsToPlay.Length];

				int nOfClips = clipsToPlay.Length;
				for (int i = 0; i < nOfClips; i++)
				{
					var currentAudioCaption = clipsToPlay[i].Caption;
					if (currentAudioCaption.Visualise && !audioCue.looping)
					{
						captionEmitterArray[i] = _pool.Request();
						if (captionEmitterArray[i] != null)
						{
							position.y += i* _spaceBetweenCaptions;
							captionEmitterArray[i].Display(currentAudioCaption, position);
							AddCaptionToOffscreenIndicatorWatchList(captionEmitterArray[i]);

							StartCoroutine(CleanEmitter(captionEmitterArray[i], currentAudioCaption.Duration));
						}
					}
				} 
			}

			return default;
		}

		private IEnumerator CleanEmitter(CaptionEmitter captionEmitter, float duration)
		{
			yield return new WaitForSeconds(duration);

			RemoveCaptionFromOffscreenIndicatorWatchList(captionEmitter);
			_pool.Return(captionEmitter);
		}

		private void AddCaptionToOffscreenIndicatorWatchList(CaptionEmitter target)
		{
			if (MainCamera == null)
			{
				MainCamera = GameObject.Find("Main Camera").GetComponent<UnityEngine.Camera>();
			}

			target.GetOffscreentTargetIndicator().InitialiseTargetIndicator(target.gameObject, MainCamera);
			captionEmmiters.Add(target);
		}

		private void RemoveCaptionFromOffscreenIndicatorWatchList(CaptionEmitter target)
		{
			captionEmmiters.Remove(target);
		}
	}
}
