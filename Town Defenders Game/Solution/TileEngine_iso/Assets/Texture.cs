using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Iso_Tile_Engine;

namespace Engine.Assets
{
    public class Texture
    {
        public Rectangle SourceRectangle;
        public Texture2D SourceTexture;
        public int Width;
        public int Height;
        public Point AlignmentOffset;

        #region Constructers
        /// <summary>
        /// Texture with everything specified
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="alignmentOffsetX"></param>
        /// <param name="alignmentOffsetY"></param>
        public Texture(Texture2D texture, int x, int y, int width, int height, int alignmentOffsetX, int alignmentOffsetY)
        {
            this.SourceTexture = texture;
            this.SourceRectangle = new Rectangle(x, y, width, height);
            this.Width = SourceRectangle.Width;
            this.Height = SourceRectangle.Height;
            this.AlignmentOffset = new Point(alignmentOffsetX, alignmentOffsetY);
        }

        // A tile
        /// <summary>
        /// Texture With same Width and Height as Tile
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Texture(Texture2D texture, int x, int y)
            : this(texture, x, y, TileInfo.TileWidth, TileInfo.TileHeight, 0, 0)
        {
        }

        // All the texture2D
        /// <summary>
        /// Texture that uses all the Texture2D given. No Alignment
        /// </summary>
        /// <param name="texture"></param>
        public Texture(Texture2D texture)
            : this(texture, 0, 0, texture.Width, texture.Height, 0, 0)
        {
        }

        //// All the texture2D with alignment
        ///// <summary>
        ///// Texture that uses all the Texture2D given with Alignment
        ///// </summary>
        ///// <param name="texture"></param>
        ///// <param name="alignmentOffsetX"></param>
        ///// <param name="alignmentOffsetY"></param>
        //public Texture(Texture2D texture, int alignmentOffsetX, int alignmentOffsetY)
        //    : this(texture, 0, 0, texture.Width, texture.Height, alignmentOffsetX, alignmentOffsetY)
        //{
        //}

        // All the texture2D with alignment preset mode
        /// <summary>
        /// Texture that uses all the Texture2D given with Alignment preset (Center, Left, Right)
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="mode"></param>
        public Texture(Texture2D texture, AlignmentMode mode)
            : this(texture)
        {
            if (mode == AlignmentMode.Center)
                this.AlignmentOffset = new Point(-(Width - TileInfo.TileWidth) / 2, Height - TileInfo.TileHeight);
        }
        #endregion

    }

}
