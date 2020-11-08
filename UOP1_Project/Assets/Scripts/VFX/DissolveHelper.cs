using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHelper : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _dissolveParticles;
    [SerializeField]
    MeshRenderer _renderer;
    [SerializeField]
    float dissolveTime = 1;

    public InputReader inputReader;

    MaterialPropertyBlock materialPropertyBlock;

    void Start() {
        if (materialPropertyBlock == null) 
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }
        setParticleSystem();
    }

    public void triggerDissolve() 
    {
        StartCoroutine(DissolveCoroutine());
    }

    void OnValidate() 
    {
        setParticleSystem();
    }

    void setParticleSystem()
    {
        var mainModule = _dissolveParticles.main;
        mainModule.duration = dissolveTime - 0.3f;
    }

    public IEnumerator DissolveCoroutine() 
    {
        float normalizedDeltaTime = 0;

        _dissolveParticles.Play();
        while(normalizedDeltaTime < dissolveTime) 
        {
            // dissolve logic
            normalizedDeltaTime += Time.deltaTime;
            float remappedValue = VFXUtil.RemapValue(normalizedDeltaTime, 0, dissolveTime, 0, 1);
            materialPropertyBlock.SetFloat("_Dissolve", remappedValue);
            _renderer.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
    }
}
