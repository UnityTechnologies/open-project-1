using System.Collections.Generic;
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
	[SerializeField] private Texture2D _defaultEyeTexture = default;
	[SerializeField] private Texture2D _defaultMouthShape = default;
	[SerializeField] private string    _defaultAnimClipTitle = default;
	[SerializeField] private Material _eyeMaterial = default;
	[SerializeField] private Material _mouthMaterial = default;

	private HashSet<Animator> _animator = new HashSet<Animator>();

	// This will add an animator to the list of animators that we gonna control.
	public void RegisterAnimator(Animator animator)
	{
		_animator.Add(animator);
	}

	// This will remove an animator from our control.
	public void UnregisterAnimator(Animator animator)
	{
		_animator.Remove(animator);
	}

	public void SetDefaultMouthTexture()
	{
		_mouthMaterial.mainTexture = _defaultMouthShape;
	}

	public void SetDefaultEyeTexture()
	{
		_eyeMaterial.mainTexture = _defaultEyeTexture;
	}

	public void SetMouthTexture(Texture2D mouthShape)
	{
		_mouthMaterial.mainTexture = mouthShape;
	}

	public void SetEyeTexture(Texture2D eyeShape)
	{
		_eyeMaterial.mainTexture = eyeShape;
	}

	public void TransitionToDefaultAnimClip(float transitionTime = 0.0f)
	{
		foreach (Animator anim in _animator)
		{
			if (anim)
				anim.CrossFade(_defaultAnimClipTitle, transitionTime);
		}
	}

	public void TransitionToAnimatorClip(int hash, float transitionTime = 0.0f)
	{
		foreach (Animator anim in _animator)
		{
			if (anim)
				anim.CrossFade(hash, transitionTime);
		}
	}
}
