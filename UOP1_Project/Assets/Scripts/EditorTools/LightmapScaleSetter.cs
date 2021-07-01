using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapScaleSetter : MonoBehaviour
{
    [SerializeField] private float _lightmapScale = 1f;

#if UNITY_EDITOR
	// Called when the Lightmap Scale field is changed in the component editor
	private void OnValidate()
    {
        // Clamp the lightmap scale to the range [0,1]
        if (_lightmapScale < 0f)
        {
            _lightmapScale = 0f;
        }

        // Update lightmap scale for MeshRenderers within component and all descendants
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.scaleInLightmap = _lightmapScale;
        }
    }
#endif
}
