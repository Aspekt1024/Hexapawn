using UnityEngine;

namespace AspektML
{
    public static class Layers
    {
        public enum ObjectLayers
        {
            Pawn, Tile
        }

        public static int GetMask(ObjectLayers layer)
        {
            return 1 << LayerMask.NameToLayer(layer.ToString());
        }

        public static int GetMask(ObjectLayers[] layers)
        {
            int mask = 0;
            foreach (var layer in layers)
            {
                mask |= GetMask(layer);
            }
            return mask;
        }
    }
}
