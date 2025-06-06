using UnityEngine;

namespace EndlessRoad
{
    public static class GameObjectExt
    {
        public static bool IsInLayer(this GameObject go, LayerMask layerMask)
        {
            //return layerMask == (layerMask | 1 << go.layer);
            return ((layerMask.value & (1 << go.layer)) > 0);
        }
    }
}
