using System;
using System.Collections.Generic;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameLogic.Selection;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Structures.Buildings
{
    public struct SegDepth
    {
        public Rectangle SegRect;
        public float Depth;
        public SegDepth(Rectangle segRect, float depth)
        {
            Depth = depth;
            SegRect = segRect;
        }
    }

    public abstract class BuildingBase : BaseGameObject, ISelectable, IEntity
    {

        public Texture Texture;
        public Texture SelectionLineTexture;
        public Point Location; // cell location of closest angle to the screen
        protected List<Point> OccupiedCellsLocations = new List<Point>();
        protected TileMap MapRef;
        public GamePlayer PlayerRef;
        private readonly List<SegDepth> _imgSegmentsDepths;
        protected List<SegDepth> ImgSegmentsDepthsOrdered;
        public Vector2 BuildingOrigin;
        protected int selectionGroup;
        protected bool isSelected;

        #region Built Units Locations and initial idle animation

        public List<Vector2> BuiltUnitsLocations;
        /// <summary>
        /// used by both NextBuiltUnitLocation, NextIdleAnimation properties
        /// those properties must be calles in a respective way.
        /// </summary>
        private int _builtUnitIndex = 0;
        public Vector2 NextBuiltUnitLocation
        {
            get
            {
                if (_builtUnitIndex == BuiltUnitsLocations.Count)
                    _builtUnitIndex = 0;
                return BuiltUnitsLocations[_builtUnitIndex];
            }
        }
        
        public string NextIdleAnimation
        {
            get
            {
                if (_builtUnitIndex == BuiltUnitsLocations.Count)
                    _builtUnitIndex = 0;
                return InitialIdleAnimation[_builtUnitIndex++];
            }
        }

        public List<string> InitialIdleAnimation;

        #endregion


        protected BuildingBase(string identifier, GameWorld gameWorldRef, GamePlayer playerRef, Point location)
        {
            Texture = GameGraphics.GetTexture(identifier);
            Location = location;
            GameWorldRef = gameWorldRef;
            MapRef = gameWorldRef.Map;
            PlayerRef = playerRef;

            var rowOffset = (MapRef.MapCellAt(location).OnOddRow) ? TileInfo.OddRowXOffset : 0;

            BuildingOrigin =
                new Vector2((location.X * TileInfo.TileStepX) + rowOffset + Texture.AlignmentOffset.X,
                    location.Y * TileInfo.TileStepY - Texture.AlignmentOffset.Y);

            #region Build Segments of the texture..
            var imgSegmentsF = new List<Rectangle>();
            var imgSegmentsB = new List<Rectangle>();


            var segWidth = TileInfo.TileWidth / 2; // seg_width = 32

            var doubleDivision = (Texture.SourceRectangle.Width / 2.0) / segWidth;
            var intDivision = (double)((int)doubleDivision);
            // check if image width not devided by half of tile width (32)
            const double epsilon = 0.01f;
            var isOverloaded = (!(Math.Abs(doubleDivision - intDivision) < epsilon));



            int height = Texture.SourceRectangle.Height;
            int mid = Texture.SourceRectangle.Width / 2;


            int forwardIndexer = mid;
            int backwardIndexer = mid - segWidth;

            for (int i = 0; i < intDivision; i++)
            {
                var rectForward = new Rectangle(forwardIndexer, Texture.SourceRectangle.Y, segWidth, height);
                var rectBackward = new Rectangle(backwardIndexer, Texture.SourceRectangle.Y, segWidth, height);

                imgSegmentsB.Add(rectBackward);
                imgSegmentsF.Add(rectForward);

                forwardIndexer += segWidth;
                backwardIndexer -= segWidth;
            }

            // next, using latest value of forward_indexer
            if (isOverloaded)
            {
                var leftSegWidth = mid - (int)intDivision * segWidth;
                var lastRectRight = new Rectangle(/**/forwardIndexer/**/, Texture.SourceRectangle.Y, leftSegWidth, height);
                var lastRectLeft = new Rectangle(Texture.SourceRectangle.X, Texture.SourceRectangle.Y, leftSegWidth, height);

                imgSegmentsB.Add(lastRectLeft);
                imgSegmentsF.Add(lastRectRight);
            }

            // setting depths of segments..
            var indexer = location;
            _imgSegmentsDepths = new List<SegDepth>();


            // first forward and backward segments have same depth as cell at this building's Location
            var depth = MapRef.MapCellAt(location).DrawDepth;
            _imgSegmentsDepths.Add(new SegDepth(imgSegmentsF[0], depth));
            _imgSegmentsDepths.Add(new SegDepth(imgSegmentsB[0], depth));

            // moving to the tiles on the right..
            for (var i = 1; i < imgSegmentsF.Count; i++)
            {
                indexer = indexer.WalkTo(Direction.NE);
                depth = MapRef.MapCellAt(indexer).DrawDepth;
                _imgSegmentsDepths.Add(new SegDepth(imgSegmentsF[i], depth));
            }

            //resetting
            indexer = location;


            // moving to the tiles on the left..
            for (int i = 1; i < imgSegmentsB.Count; i++)
            {
                indexer = indexer.WalkTo(Direction.NW);
                depth = MapRef.MapCellAt(indexer).DrawDepth;
                _imgSegmentsDepths.Add(new SegDepth(imgSegmentsB[i], depth));
            }

            // constructing an ordered version of the img_segments_depths list..
            ImgSegmentsDepthsOrdered = new List<SegDepth>();
            for (int i = _imgSegmentsDepths.Count - 1; i >= _imgSegmentsDepths.Count - (imgSegmentsB.Count - 1); i--)
            {
                ImgSegmentsDepthsOrdered.Add(_imgSegmentsDepths[i]);
            }
            ImgSegmentsDepthsOrdered.Add(_imgSegmentsDepths[1]);
            ImgSegmentsDepthsOrdered.Add(_imgSegmentsDepths[0]);

            for (int i = 2; i < (imgSegmentsF.Count - 1) + 2; i++)
            {
                ImgSegmentsDepthsOrdered.Add(_imgSegmentsDepths[i]);
            }

            ///
            /// now, img_segments_depths_ordered is constructed properly
            /// so that it can be used to access and / or modify the segments of 
            /// the building in the child classes of this class
            /// 


            #endregion

            #region Generating Built Units Locations and idle animations
            BuiltUnitsLocations = new List<Vector2>();
            var cells = new List<Point>();
            var index = Location;
            index = index.WalkTo(Direction.S).WalkTo(Direction.NE)
                .WalkTo(Direction.NE).WalkTo(Direction.NE);
            cells.Add(index);
            index = index.WalkTo(Direction.SW);
            cells.Add(index);
            index = index.WalkTo(Direction.SW);
            cells.Add(index);
            index = index.WalkTo(Direction.SW);
            cells.Add(index);
            index = index.WalkTo(Direction.NW);
            cells.Add(index);
            index = index.WalkTo(Direction.NW);
            cells.Add(index);
            index = index.WalkTo(Direction.NW);
            cells.Add(index);
            _builtUnitIndex = 0;
            foreach (var point in cells)
            {
                BuiltUnitsLocations.Add(MapRef.MapCellAt(point).CenterPosition);
            }


            // setting initial idle animations to look away from building
            InitialIdleAnimation = new List<string>();
            InitialIdleAnimation.Add("Idle_SE");
            InitialIdleAnimation.Add("Idle_SE");
            InitialIdleAnimation.Add("Idle_SE");
            InitialIdleAnimation.Add("Idle_S");
            InitialIdleAnimation.Add("Idle_SW");
            InitialIdleAnimation.Add("Idle_SW");
            InitialIdleAnimation.Add("Idle_S");
            InitialIdleAnimation.Add("Idle_SW");
            InitialIdleAnimation.Add("Idle_SW");
            #endregion
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //TODO draw only if seen
            var v2 = MapRef.MapCellAt(Location).CenterPosition;
            var point = new Point((int)v2.X, (int)v2.Y);
            if (!Camera.ExtendedBoundingBox.Contains(point))
                return;

            foreach (var segment in ImgSegmentsDepthsOrdered)
            {
                spriteBatch.Draw(
                Texture.SourceTexture,
                Camera.WorldToScreen(
                    new Vector2(BuildingOrigin.X + segment.SegRect.X, BuildingOrigin.Y + segment.SegRect.Y)),  // TODO: Debug
                segment.SegRect,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                segment.Depth);
            }

            if (isSelected)
                DrawSelectionLine(spriteBatch);
            
        }

        protected void DrawSelectionLine(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(
               SelectionLineTexture.SourceTexture,
               Camera.WorldToScreen(BuildingOrigin),
               SelectionLineTexture.SourceRectangle,
               Color.White,
               0.0f,
               Vector2.Zero,
               1.0f,
               SpriteEffects.None,
               1.0f - 0.0000001f); //TODO change buildong draw depth

        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateSelectionHandling();
        }

        private void UpdateSelectionHandling()
        {
            GameWorldRef.SelectionManager.ManageSelection(this);
        }


        #region ISelectable Implementation
        public int SelectionGroup
        {
            get
            {
                return selectionGroup;
            }
            set
            {
                selectionGroup = value;
            }
        }


        public virtual void Select()
        {
            isSelected = true;
        }

        public virtual void DeSelect()
        {
            isSelected = false;
        }

        public virtual bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }


        public Rectangle SelectBounds
        {
            get
            {
                return new Rectangle(
                    (int)BuildingOrigin.X,
                    (int)BuildingOrigin.Y,
                    Texture.SourceRectangle.Width,
                    Texture.SourceRectangle.Height);
            }

        }
        #endregion

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
