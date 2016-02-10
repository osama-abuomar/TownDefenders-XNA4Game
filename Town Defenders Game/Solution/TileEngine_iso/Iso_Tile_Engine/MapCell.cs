using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.Components.GameObjects;
using Texture = Engine.Assets.Texture;

namespace Engine.Iso_Tile_Engine
{
    public class MapCell
    {
        //TODO: soon to be lists of visuals
        public float DrawDepth; // set in the TileMap constructor
        public List<Texture> BaseTiles = new List<Texture>();
        public List<Texture> HeightTiles = new List<Texture>();
        public List<Texture> TopperTiles = new List<Texture>();
        public bool Walkable { get; set; }
        public Texture SlopeMap { get; set; }
        public Point Location;
        public bool OnOddRow;


        public Vector2 CenterPosition
        {
            get
            {
                float xCoor = Location.X * TileInfo.TileStepX + (TileInfo.TileWidth) / 2;
                float yCoor = Location.Y * TileInfo.TileStepY + (TileInfo.TileHeight) / 2 + (TileInfo.TileHeight) / 4;
                if (OnOddRow) xCoor += TileInfo.OddRowXOffset;
                return new Vector2(xCoor, yCoor);
            }
        }
       
        public void AddBaseTile(string textureIdentifier)
        {
            BaseTiles.Add(GameGraphics.GetTexture(textureIdentifier));
        }

        public void AddHeightTile(string textureIdentifier)
        {
            HeightTiles.Add(GameGraphics.GetTexture(textureIdentifier));
        }

        public void AddTopperTile(string textureIdentifier)
        {
            TopperTiles.Add(GameGraphics.GetTexture(textureIdentifier));
        }

        public MapCell()
        {
            Walkable = true;
        }

        public MapCell(string identifier, bool walkable)
        {
            AddBaseTile(identifier);
            Walkable = walkable;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int rowOffset = 0;
            if (OnOddRow) rowOffset = TileInfo.OddRowXOffset;

            foreach (var tileTexture in BaseTiles)
            {
                spriteBatch.Draw(
                    tileTexture.SourceTexture,
                    Camera.WorldToScreen(
                        new Vector2((Location.X * TileInfo.TileStepX) + rowOffset, 
                            Location.Y * TileInfo.TileStepY)),
                    tileTexture.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    1.0f); // furthest to the screen
            }


            int heightRow = 0;

            foreach (var tileTexture in HeightTiles)
            {
                spriteBatch.Draw(
                    tileTexture.SourceTexture,
                    Camera.WorldToScreen(
                        new Vector2(
                            (Location.X * TileInfo.TileStepX) + rowOffset,
                            Location.Y * TileInfo.TileStepY - (heightRow * TileInfo.HeightTileOffset))),
                    tileTexture.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    DrawDepth - (heightRow * TileEngineInfo.HeightRowDepthMod));
                heightRow++;
            }


            foreach (var tileTexture in TopperTiles)
            {
                spriteBatch.Draw(
                    tileTexture.SourceTexture,
                    Camera.WorldToScreen(
                        new Vector2((Location.X * TileInfo.TileStepX) + rowOffset, Location.Y * TileInfo.TileStepY)),
                    tileTexture.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    DrawDepth - (heightRow * TileEngineInfo.HeightRowDepthMod));
            }
        }
    }
}
