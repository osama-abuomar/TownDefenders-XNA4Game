using System;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameLogic.Selection;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Structures.Farm
{
    public abstract class FarmBase:BaseGameObject, ISelectable, IEntity
    {
        public Texture Texture;
        public Point Location;
        private readonly float _drawDepth;
        private int _selectionGroup;
        private bool _isSelected;
        /// <summary>
        /// relative to world (not screen)
        /// </summary>
        protected Vector2 FarmOrigin;
        private readonly GameWorld _gameWorldRef;
        protected TileMap MapRef;
        // no need for Origion like buildings, since trhe depth is solid and far from screen.
        private readonly float _rowOffset;
        protected Texture SelectionLineTexture;
        protected GamePlayer PlayerRef;

        public FarmBase(string identifier, GameWorld gameWorldRef, GamePlayer playerRef, Point location)
        {
            Texture = GameGraphics.GetTexture(identifier);
            Location = location;
            _gameWorldRef = gameWorldRef;
            PlayerRef = playerRef;
            MapRef = _gameWorldRef.Map;

            _drawDepth = 1.0f - TileEngineInfo.HeightRowDepthMod;

            _rowOffset = (MapRef.MapCellAt(location).OnOddRow) ? TileInfo.OddRowXOffset : 0;

            FarmOrigin =
               new Vector2((Location.X * TileInfo.TileStepX) + _rowOffset + Texture.AlignmentOffset.X,
                   Location.Y * TileInfo.TileStepY - Texture.AlignmentOffset.Y);


        }


        public virtual void Update(GameTime gameTime)
        {
            UpdateSelectionHandling();
        }

        private void UpdateSelectionHandling()
        {
            _gameWorldRef.SelectionManager.ManageSelection(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               this.Texture.SourceTexture,
               Camera.WorldToScreen(
                   new Vector2((Location.X * TileInfo.TileStepX) + _rowOffset + Texture.AlignmentOffset.X, Location.Y * TileInfo.TileStepY - Texture.AlignmentOffset.Y)),
               this.Texture.SourceRectangle,
               Color.White,
               0.0f,
               Vector2.Zero,
               1.0f,
               SpriteEffects.None,
               _drawDepth);


            if (_isSelected)
            {
                DrawSelectionLine(spriteBatch);
            }
        }

        protected void DrawSelectionLine(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(
               SelectionLineTexture.SourceTexture,
               Camera.WorldToScreen(FarmOrigin),
               SelectionLineTexture.SourceRectangle,
               Color.White,
               0.0f,
               Vector2.Zero,
               1.0f,
               SpriteEffects.None,
               0.8f); //TODO change buildong draw depth

        }

        public int SelectionGroup
        {
            get { return _selectionGroup; }
            set { _selectionGroup = value; }
        }

        public virtual void Select()
        {
            _isSelected = true;
            Console.WriteLine("farm selected");
        }

        public virtual void DeSelect()
        {
            _isSelected = false;
            Console.WriteLine("farm selected");
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public Rectangle SelectBounds
        {
            get
            {
                return new Rectangle(
                    (int)FarmOrigin.X,
                    (int)FarmOrigin.Y,
                    Texture.SourceRectangle.Width,
                    Texture.SourceRectangle.Height);
            }
        }

        #region IEntity Implementation
        public int Health { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int MaxHealth { get; set; }
        public int MaxDefense { get; set; }
        public int MaxAttack { get; set; }
        public abstract string UnitDiscription { get; }
        public string PlayerDiscription
        {
            get { return "Player: " + PlayerRef.Name; }
        }

        public string HealthDiscription
        {
            get { return "Health: " + Health; }
        }

        public string DefenseDiscription
        {
            get { return "Defense: " + Defense; }
        }

        public string AttackDiscription
        {
            get { return "Attack: " + Attack; }
        }
        #endregion
    }
}
