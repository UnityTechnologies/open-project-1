using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHelper : MonoBehaviour
{
    [SerializeField] ParticleSystem _dissolveParticles;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] float _dissolveTime = 1f;

    private MaterialPropertyBlock _materialPropertyBlock;

	private void Start() {
        if (_materialPropertyBlock == null) 
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        SetParticleSystemDuration();
    }

    public void TriggerDissolve()
    {
        StartCoroutine(DissolveCoroutine());
    }

	private void OnValidate() 
    {
        SetParticleSystemDuration();
    }

	private void SetParticleSystemDuration()
    {
        ParticleSystem.MainModule mainModule = _dissolveParticles.main;
        mainModule.duration = _dissolveTime - 0.3f;
    }

    public IEnumerator DissolveCoroutine() 
    {
        float normalizedDeltaTime = 0;

        _dissolveParticles.Play();

        while(normalizedDeltaTime < _dissolveTime) 
        {
            normalizedDeltaTime += Time.deltaTime;
            float remappedValue = VFXUtil.RemapValue(normalizedDeltaTime, 0, _dissolveTime, 0, 1);
            _materialPropertyBlock.SetFloat("_Dissolve", remappedValue);
            _renderer.SetPropertyBlock(_materialPropertyBlock);

            yield return null;
        }
    }
}
