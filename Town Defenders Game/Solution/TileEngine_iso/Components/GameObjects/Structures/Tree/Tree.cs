using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Structures.Tree
{
    public class Tree
    {
        public Texture texture;
        private readonly GameWorld _gameWorldRef;
        public Point Location;
        protected TileMap MapRef;
        public bool IsSelected = false;
        public Rectangle SelectionBox;
        public Vector2 Position;
        int rowOffset;
        float depth;

        public Tree(string Identifier, GameWorld gameWorldRef, Point Location)
        {
            this.texture = GameGraphics.GetTexture(Identifier);
            _gameWorldRef = gameWorldRef;
            this.Location = Location;
            this.MapRef = _gameWorldRef.Map;

             rowOffset = (MapRef.MapCellAt(Location).OnOddRow) ? TileInfo.OddRowXOffset : 0;
             depth = MapRef.MapCellAt(Location).DrawDepth;

            // make tree place unwalkable..
            MapRef.MapCellAt(Location).Walkable = false;
            // add tree shadow to base tiles of the occupied cell..
            MapRef.MapCellAt(Location).AddTopperTile("tree_shadow");
            Position = new Vector2((Location.X * TileInfo.TileStepX) + rowOffset + texture.AlignmentOffset.X, 
                Location.Y * TileInfo.TileStepY - texture.AlignmentOffset.Y);
            SelectionBox = new Rectangle((int)Position.X, (int)Position.Y, texture.SourceRectangle.Width, texture.SourceRectangle.Height);
              
          

        }

    

        public void Draw(SpriteBatch spriteBatch)
        {
            //TODO draw tree if seen
            var v2 = MapRef.MapCellAt(Location).CenterPosition;
            var point = new Point((int) v2.X, (int) v2.Y);
            if (!Camera.ExtendedBoundingBox.Contains(point))
                return;

            spriteBatch.Draw(
               texture.SourceTexture,
               Camera.WorldToScreen(
                   new Vector2((Location.X * TileInfo.TileStepX) + rowOffset + texture.AlignmentOffset.X, Location.Y * TileInfo.TileStepY - texture.AlignmentOffset.Y)),
               this.texture.SourceRectangle,
               Color.White,
               0.0f,
               Vector2.Zero,
               1.0f,
               SpriteEffects.None,
               depth + TreeDepthOffsetFromTileDepth);
        }

        protected float TreeDepthOffsetFromTileDepth = -0.00001f;

    }
}
