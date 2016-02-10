
namespace Engine.Iso_Tile_Engine
{
    public static class TileEngineInfo
    {
        /// <summary>
        /// Exposes how the tiles that are actually drawn (for better optimisation)
        /// </summary>
        public static int SquaresAcross = 22 +1; // for 1366
        public static int SquaresDown = 49 +1;   // for 768
        public static int BaseOffsetX = -32;
        public static int BaseOffsetY = -48;
        public static float HeightRowDepthMod = 0.00001f;

     
    }
}
