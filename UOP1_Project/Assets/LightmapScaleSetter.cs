using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapScaleSetter : MonoBehaviour
{
    [SerializeField] private float _scale = 1f;

    private void OnValidate()
    {
        if (_scale < 0f)
        {
            _scale = 0f;
        }
        else if (_scale > 1f)
        {
            _scale = 1f;
        }
        Debug.Log("Lightmap scale set to " + _scale);

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log(meshRenderers.Length + " mesh renderers found in component's children.");
    }
}
