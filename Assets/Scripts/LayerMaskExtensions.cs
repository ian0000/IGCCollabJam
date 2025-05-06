using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool LayerInMask(this LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }
}
