using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Assets;
using Engine.Components.GameLogic.Actions;
using Engine.Components.GameLogic.Movement;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameLogic.Selection;
using Engine.Components.GameObjects.Views;
using Microsoft.Xna.Framework;
using Engine.Animation;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework.Graphics;
using Engine.Components.GameLogic;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Characters
{


    public abstract class CreatureBase: BaseGameObject ,ISelectable, IMovable, IEntity
    {
      
        public string CurrentAnimation
        {
            get { return Animation.CurrentAnimation; }
            set { Animation.CurrentAnimation = value; }
        }
        /// <summary>
        /// possible actions made by this unit
        /// constructed on the child class constructors
        /// </summary>
        protected ActionManager Actions;
        protected  SpriteAnimation Animation;
        /// <summary>
        /// the sprite sheet used by this unit
        /// assigned in the child classes constructors
        /// </summary>
        protected  Texture AnimationSet;
        protected  Texture CharSelect;
        private bool _isSelected = false;
        private readonly TileMap _mapRef;
        private int _selectionGroup;
        private Vector2 _currentMoveDir;
        private Vector2 _previousMovDir;
        protected GamePlayer PlayerRef;
        /// <summary>
        /// the indicator line on top of the unit
        /// it may indecate: Health, Capacity of something ...etc
        /// </summary>
        protected StatusLine HealthStatusLine;
        /// <summary>
        /// the drawing offset of the StatusLine
        /// it may differ if the unit size differ
        /// </summary>
        private Vector2 _healthStatusLineOffset;
        /// <summary>
        /// general purpose timer
        /// </summary>
        public long Timer = 0;
        public bool TimerOn = false;
       

        protected CreatureBase(string animationIdentifier, 
            int frameWidth, int frameHeight, int frames,
            GameWorld gameWorldRef, GamePlayer playerRef, Vector2 position, float animationSpeed)
        {

            PlayerRef = playerRef;
            GameWorldRef = gameWorldRef;
            _mapRef = gameWorldRef.Map ;
           
            // walking animation is unified in all creatures
            #region setting appropriate walking animation
            AnimationSet = GameGraphics.GetTexture(animationIdentifier);
            Animation = new SpriteAnimation(AnimationSet.SourceTexture);

            Animation.AddAnimation("Walk_E", 0, frameHeight * 0, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_N", 0, frameHeight * 1, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_NE", 0, frameHeight * 2, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_NW", 0, frameHeight * 3, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_S", 0, frameHeight * 4, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_SE", 0, frameHeight * 5, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_SW", 0, frameHeight * 6, frameWidth, frameHeight, frames, animationSpeed);
            Animation.AddAnimation("Walk_W", 0, frameHeight * 7, frameWidth, frameHeight, frames, animationSpeed);

            Animation.AddAnimation("Idle_E", 3 * frameWidth, frameHeight * 0, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_N", 3 * frameWidth, frameHeight * 1, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_NE", 3 * frameWidth, frameHeight * 2, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_NW", 3 * frameWidth, frameHeight * 3, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_S", 3 * frameWidth, frameHeight * 4, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_SE", 3 * frameWidth, frameHeight * 5, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_SW", 3 * frameWidth, frameHeight * 6, frameWidth, frameHeight, 1, 0.2f);
            Animation.AddAnimation("Idle_W", 3 * frameWidth, frameHeight * 7, frameWidth, frameHeight, 1, 0.2f);

            CurrentAnimation = "Idle_SE";

            
            Animation.IsAnimating = true;
         
            #endregion

            Position = position;
            _currentMoveDir = Vector2.Zero;
            _previousMovDir = Vector2.Zero;
            Distination = position;

            HealthStatusLine = new StatusLine(StatusLineType.Green);
            _healthStatusLineOffset = new Vector2(-13, -50);

            Actions = new ActionManager();
        }
  
        private void UpdateSelectionHandling() 
        {
            GameWorldRef.SelectionManager.ManageSelection(this);
        }

        private void UpdateMovementHandling()
        {
            GameWorldRef.MovementManager.ManageMovement(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            // update timer if on
            if (TimerOn)
            {
                Timer += gameTime.ElapsedGameTime.Milliseconds;
            } 
            // since the SelectionBox (rect) changes (moves)
            // when the unit itself moves
            UpdateSelectionBox();
            UpdateSelectionHandling();
            UpdateMovementHandling();

            Animation.Update(gameTime);
            
            
            if(IsSelected)
                HealthStatusLine.UpdateView(Health, MaxHealth, Position + _healthStatusLineOffset);

            // travese all pssible actions for this type of unit (child class)
            // and apply the action if its respective condition holds
            UpdateActionManager();
        }

        private void UpdateActionManager()
        {
            foreach (IAction action in Actions.Where(action => action.Holds))
            {
                action.Apply();
            }
        }

        private void UpdateSelectionBox()
        {
            SelectBounds = new Rectangle((int)Position.X - (48) / 2 + 8, (int)Position.Y - 48 + 16, 48 - 8 - 8, 48 - 16);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // vlad Location Indeceis
            Point vladMapPoint = _mapRef.WorldToMapCell(new Point((int)Animation.Position.X, (int)Animation.Position.Y));

            Animation.DrawDepth = _mapRef.MapCellAt(vladMapPoint.X, vladMapPoint.Y).DrawDepth
                - (_mapRef.MapCellAt(vladMapPoint.X, vladMapPoint.Y).HeightTiles.Count + 2) * TileEngineInfo.HeightRowDepthMod;

            Animation.Draw(spriteBatch, 0, -_mapRef.GetOverallHeight(Animation.Position));

            if (_isSelected)
            {
                DrawSelectionCircle(spriteBatch);
                HealthStatusLine.Draw(spriteBatch);
            }
        }

        private void DrawSelectionCircle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    CharSelect.SourceTexture,
                    Camera.WorldToScreen(new Vector2(x: Position.X - CharSelect.SourceRectangle.Width / 2, y: Position.Y - CharSelect.SourceRectangle.Height / 2 - 5)),
                    CharSelect.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    Animation.DrawDepth + 0.0000001f);// depth !

        }


        #region ISelectable Implementation
        public virtual void Select()
        {
            _isSelected = true;
            Console.WriteLine("Char " +Id+ " is Selected, g("+_selectionGroup+")");
        }

        public virtual void DeSelect()
        {
            _isSelected = false;
            Console.WriteLine("Char " + Id + " is DeSelected, g(" + _selectionGroup + ")");
        }

        public int SelectionGroup
        {
            get
            {
                return _selectionGroup;
            }
            set
            {
                _selectionGroup = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }

        public Rectangle SelectBounds { get; set; }
        #endregion

        #region IMovable Implementation

        /// <summary>
        /// reference to world - world position not screen
        /// </summary>
        public Vector2 Position
        {
            get { return Animation.Position; }
            set { Animation.Position = value; }
        }

        public float Speed { get; set; }

        public Vector2 MoveDirection
        {
            get
            {
                return _currentMoveDir;
            }
            set
            {
                _currentMoveDir = value;
            }
        }

        public Vector2 PreviousMoveDirection
        {
            get
            {
                return _previousMovDir;
            }
            set
            {
                _previousMovDir = value;
            }
        }

        public Vector2 Distination { get; set; }

        public bool CanGetMoveOrder
        {
            get
            {
                return IsSelected;
            }
            
        }

        public Vector2 CellLocation
        {
            get
            {
                var loc = _mapRef.GetCellAtWorldPoint(this.Position).Location;
                return new Vector2(loc.X, loc.Y);
            }
         
        }

        public Vector2 DrawOffset
        {
            get
            {
                return Animation.DrawOffset;
            }

        }

        public List<Vector2> MovingPath { get; set; }

        public bool IsMoving
        {
            get
            {
                return (MoveDirection != Vector2.Zero);
            }

        }

        public void UpdateMovingAnimation()
        {
            if (MoveDirectionChanged())
            {
                var animation = string.Empty;
                var x = _currentMoveDir.X;
                var y = _currentMoveDir.Y;


                const float c2 = 0.9f;

                if (x > c2)
                {
                    animation = "Walk_E";
                }
                else if (x < -c2)
                {
                    animation = "Walk_W";
                }
                else if (y < -c2)
                {
                    animation = "Walk_N";
                }
                else if (y > c2)
                {
                    animation = "Walk_S";
                }
                else if (x > 0 && y > 0)
                {
                    animation = "Walk_SE";
                }
                else if (x > 0 && y < 0)
                {
                    animation = "Walk_NE";
                }
                else if (x < 0 && y > 0)
                {
                    animation = "Walk_SW";
                }
                else if (x < 0 && y < 0)
                {
                    animation = "Walk_NW";
                }


                if (x == 0 && y == 0)
                {
                    animation = "Idle" + CurrentAnimation.Substring(4);
                }

                if (CurrentAnimation != null && CurrentAnimation != animation)
                {
                    CurrentAnimation = animation;
                }
            }
        }

        public bool MoveDirectionChanged()
        {
            if (_currentMoveDir == _previousMovDir)
                return false;
            return true;
        }

        public void MoveBy(float X, float Y)
        {
            Animation.MoveBy(X, Y);
        }

        public int CurrentPathSegment { get; set; }

        public void Stop()
        {
            MoveDirection = Vector2.Zero;
            MovingPath.Clear();
            Distination = Position;
        }
        #endregion

        #region IEntity Implementation
        public  int Health { get; set; }
        public  int Defense { get; set; }
        public  int Attack { get; set; }
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
