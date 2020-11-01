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
        if (materialPropertyBlock == null) {
            materialPropertyBlock = new MaterialPropertyBlock();
        }
    }

    public void triggerDissolve() {
        StartCoroutine(DissolveCoroutine());
    }

    public IEnumerator DissolveCoroutine() {
        float normalizedDeltaTime = 0;

        _dissolveParticles.Play();
        while(normalizedDeltaTime < dissolveTime) {

            // dissolve logic            
            materialPropertyBlock.SetFloat("_Dissolve", normalizedDeltaTime);
            _renderer.SetPropertyBlock(materialPropertyBlock);            
            
            normalizedDeltaTime += Time.deltaTime;
            yield return null;
        }
    }
}
