using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.Graphics;
using Engine.Iso_Tile_Engine;


namespace Components.GameObjects
{
    public class Tree
    {
        public Engine.Graphics.Texture texture;
        //public Point mapLocation;

        #region Constructors

        public Tree(Engine.Graphics.Texture texture)
        {
            this.texture = texture;
        }

        public Tree()
        {
            this.texture = GameGraphics.GetTexture("spring_tree");
        }

        #endregion

        /// <summary>
        /// Draws this Tree on the map
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="mapx"></param>
        /// <param name="mapy"></param>
        /// <param name="rowOffset"></param>
        /// <param name="depthOffset"></param>
        public void Draw(SpriteBatch spriteBatch, int mapx, int mapy, int rowOffset, float depthOffset)
        {
            spriteBatch.Draw(
               this.texture.SourceTexture,
               Camera.WorldToScreen(
                   new Vector2((mapx * Tile.TileStepX) + rowOffset - 64, mapy * Tile.TileStepY - 128)),
               this.texture.SourceRectangle,
               Color.White,
               0.0f,
               Vector2.Zero,
               1.0f,
               SpriteEffects.None,
               depthOffset);// depth !
        }


    }
}
