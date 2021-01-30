using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
/// <summary>
/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
/// </summary>

[CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
public class ActorSO : ScriptableObject
{
	// Public Accessors
	public LocalizedString ActorName { get => _actorName; }

	// Inspector Assigned
	[SerializeField] private LocalizedString   _actorName            = default;
	[SerializeField] private Texture2D         _defaultEyeTexture    = default;
	[SerializeField] private Texture2D         _defaultMouthShape    = default;
	[SerializeField] private string            _defaultAnimClipTitle = default;
	[SerializeField] private List<BlendTarget> _defaultEyeTargets    = default;
	[SerializeField] private List<BlendTarget> _defaultMouthTargets  = default;
	[SerializeField] private Material          _eyeMaterial          = default;
	[SerializeField] private Material          _mouthMaterial        = default;

	// Private
	private Material _backupEyeMaterial;
	private Material _backupMouthMaterial;
	private List<float> _backupBlendShapes;
	private HashSet<Animator>       _animator             = new HashSet<Animator>();
	private SkinnedMeshRenderer     _smr                  = null;
	private Dictionary<string, int> _blendShapeDictionary = new Dictionary<string, int>();

	// Record textures and blend shapes before pressing play so that they can be restored when out of play mode
	public void OnStart()
	{
		_backupEyeMaterial = new Material(Shader.Find("Unlit/Transparent"));
		_backupMouthMaterial = new Material(Shader.Find("Unlit/Transparent"));

		if (_eyeMaterial)
			_backupEyeMaterial.mainTexture = _eyeMaterial.mainTexture;

		if (_mouthMaterial)
			_backupMouthMaterial.mainTexture = _mouthMaterial.mainTexture;

		if (_smr)
		{
			Mesh mesh = _smr.sharedMesh;

			for (int i = 0; i < mesh.blendShapeCount; i++)
			{
				_backupBlendShapes.Add(_smr.GetBlendShapeWeight(i));
			}
		}
	}

	// Restore texture and blend shapes as they were before play was pressed
	public void OnReset()
	{
		if (_eyeMaterial)
			_eyeMaterial.mainTexture = _backupEyeMaterial.mainTexture;

		if (_mouthMaterial)
			_mouthMaterial.mainTexture = _backupMouthMaterial.mainTexture;

		if (_smr)
		{
			Mesh mesh = _smr.sharedMesh;

			for (int i = 0; i < mesh.blendShapeCount; i++)
			{
				_smr.SetBlendShapeWeight(i, _backupBlendShapes[i]);
			}
		}
	}

	// ---- ANIMATION CONTROL ----
	// This will register an animator to the hash set so that this script will have
	// override authority on what clip is playing ... via Animator.CrossFade()
	public void RegisterAnimator(Animator animator)
	{
		_animator.Add(animator);		
	}

	// This will remove an animator from our control
	public void UnregisterAnimator(Animator animator)
	{
		_animator.Remove(animator);
	}

	// Tell the registered animators to transition into the default clip
	public void TransitionToDefaultAnimClip(float transitionTime = 0.0f)
	{
		foreach (Animator anim in _animator)
		{
			if (anim && _defaultAnimClipTitle != null)
				anim.CrossFade(_defaultAnimClipTitle, transitionTime);
		}
	}

	// Tell the registered animators to transition into a specified clip, within
	// the transitionTime specified
	public void TransitionToAnimatorClip(int hash, float transitionTime = 0.0f)
	{
		foreach (Animator anim in _animator)
		{
			if (anim)
				anim.CrossFade(hash, transitionTime);
		}
	}

	// ---- MATERIAL MAINTEXTURE CONTROL ----
	// Sets the default mouth texture on the mouthMaterial
	public void SetDefaultMouthTexture()
	{
		_mouthMaterial.mainTexture = _defaultMouthShape;
	}

	// Sets the default eye texture on the eyeMaterial
	public void SetDefaultEyeTexture()
	{
		_eyeMaterial.mainTexture = _defaultEyeTexture;
	}

	// Sets a specific mouth texture on the mouthMaterial
	public void SetMouthTexture(Texture2D mouthShape)
	{
		_mouthMaterial.mainTexture = mouthShape;
	}

	// Sets a specific eye texture on the eyeMaterial
	public void SetEyeTexture(Texture2D eyeShape)
	{
		_eyeMaterial.mainTexture = eyeShape;
	}

	// ---- BLEND SHAPE CONTROL ----
	// This will add a skinned mesh renderer that will give this script authority over
	// the registered skinned mesh renderers blend shapes
	public void RegisterSkinnedMeshRenderer(SkinnedMeshRenderer smr)
	{
		_smr = smr;
	}

	// This will remove a skinned mesh renderer from our control
	public void UnregisterSkinnedMeshRenderer(SkinnedMeshRenderer smr)
	{
		_smr = null;
	}

	// Applies the default blend shapes for the eyes
	public void SetDefaultEyeBlendShape()
	{
		foreach (BlendTarget bt in _defaultEyeTargets)
		{
			SetBlendTarget(bt.BlendShape, bt.BlendWeight);
		}
	}

	// Applies the default blend shapes for the mouth
	public void SetDefaultMouthBlendShape()
	{
		foreach (BlendTarget bt in _defaultMouthTargets)
		{
			SetBlendTarget(bt.BlendShape, bt.BlendWeight);
		}
	}

	private void BuildBlendShapeDictionary()
	{
		// Start off by adding entries for the default blend shapes
		foreach (BlendTarget bt in _defaultEyeTargets)
		{
			// We need to reference blend shapes by the indexes that the skinned mesh renderer
			// knows them by...
			int result;
			if (!_blendShapeDictionary.TryGetValue(bt.BlendShape, out result))
			{
				_blendShapeDictionary.Add(bt.BlendShape, GetBlendIndex(bt.BlendShape));
			}
		}

		// Do the same for the default mouth shapes
		foreach (BlendTarget bt in _defaultMouthTargets)
		{
			int result;
			if (!_blendShapeDictionary.TryGetValue(bt.BlendShape, out result))
			{
				_blendShapeDictionary.Add(bt.BlendShape, GetBlendIndex(bt.BlendShape));
			}
		}
	}

	private int GetBlendIndex(string blendName)
	{
		if (_smr == null)
			return -1;

		Mesh mesh = _smr.sharedMesh;

		int blendIndex = -1;
		for (int i = 0; i < mesh.blendShapeCount; i++)
		{
			if (mesh.GetBlendShapeName(i) == blendName)
			{
				blendIndex = i;
				break;
			}
		}

		return blendIndex;
	}

	// Returns the blend weight for a given blend shape name
	public float GetBlendWeight(string blendName)
	{
		if (_smr == null)
			return -1.0f;

		// If we don't have a key for this entry yet, add one
		int blendIndex;
		if (!_blendShapeDictionary.TryGetValue(blendName, out blendIndex))
		{
			blendIndex = GetBlendIndex(blendName);
			_blendShapeDictionary.Add(blendName, blendIndex);
		}

		if (blendIndex == -1)
			return -1.0f;

		// Now ask the skinned mesh render what the blend shape weight is for this index
		float blendWeight = _smr.GetBlendShapeWeight(blendIndex);
		return blendWeight;
	}

	// Sets a specific blend shape weight
	public void SetBlendTarget(string blendName, float blendWeight)
	{
		if (_smr == null)
			return;

		// If we don't have a key for this entry yet, add one
		int blendIndex;
		if (!_blendShapeDictionary.TryGetValue(blendName, out blendIndex))
		{
			blendIndex = GetBlendIndex(blendName);
			_blendShapeDictionary.Add(blendName, blendIndex);
		}

		if (blendIndex == -1)
			return;

		// Now set the set this blend index with the target blend weight
		_smr.SetBlendShapeWeight(blendIndex, blendWeight);
	}
}
