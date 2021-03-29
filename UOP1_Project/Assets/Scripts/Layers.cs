//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UOP1
{
    using System;
    
    /// <summary>
    /// Use this enum in place of layer names in code / scripts.
    /// </summary>
    /// <example>
    /// <code>
    /// if (other.gameObject.layer == Layer.Characters) {
    ///     Destroy(other.gameObject);
    /// }
    /// </code>
    /// </example>
    public enum Layer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,
        PostProcessing = 8,
        Characters = 9,
        HideWaterMask = 10,
        OccludingObjects = 11,
        OccludedTransparents = 12,
        TreeLeaves = 13,
    }
    /// <summary>
    /// Use this enum in place of layer mask values in code / scripts.
    /// </summary>
    /// <example>
    /// <code>
    /// if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, (int) (LayerMasks.Characters | LayerMasks.Water)) {
    ///     Debug.Log("Did Hit");
    /// }
    /// </code>
    /// </example>
    [FlagsAttribute()]
    public enum LayerMasks
    {
        Default = 1,
        TransparentFX = 2,
        IgnoreRaycast = 4,
        Water = 16,
        UI = 32,
        PostProcessing = 256,
        Characters = 512,
        HideWaterMask = 1024,
        OccludingObjects = 2048,
        OccludedTransparents = 4096,
        TreeLeaves = 8192,
    }
}
