
using Engine.Iso_Tile_Engine;

namespace Engine.Components.GameObjects.Clouds
{
    public class CloudInfo
    {
        public static int CloudShadowOffsetX = 4 * TileInfo.TileWidth;
        public static int CloudShadowOffsetY = 10 * TileInfo.TileHeight;
        public static float CloudsDrawDepth = 0.2f;
        public static float CloudsShadowDrawDepth = 0.2f + 0.00001f;
        //public static int CloudLocationOffsetX = +900;
        //public static int CloudLocationOffsetY = -950;



    }
}
