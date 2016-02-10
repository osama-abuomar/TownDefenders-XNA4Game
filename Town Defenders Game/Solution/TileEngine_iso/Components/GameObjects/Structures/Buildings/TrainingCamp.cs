using Engine.Animation;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Structures.Buildings
{
    public class TrainingCamp : BuildingBase
    {
        private readonly SpriteAnimation _smoke;

     

        public TrainingCamp(GameWorld gameWorldRef, GamePlayer playerRef, Point Location)
            : base("building6", gameWorldRef, playerRef, Location)
        {
            // filling the occupied_cells_locations List 
            // (the purpose is to make these cells not walkable)
            #region Occupied Cells Work

            const int cellsEast = 3;
            const int cellsWest = 4;

            Point westIndexer = Location;
            Point eastIndexer = Location;

            for (int i = 0; i < cellsWest; i++)
            {
                for (int j = 0; j < cellsEast; j++)
                {
                    OccupiedCellsLocations.Add(eastIndexer);
                    eastIndexer = eastIndexer.WalkTo(Direction.NE);
                }
                westIndexer = westIndexer.WalkTo(Direction.NW);
                eastIndexer = westIndexer;
            }
            


            // add the irregular occupied cells..
            Point indexer = Location;
            indexer = indexer.WalkTo(Direction.SW);
            indexer = indexer.WalkTo(Direction.NW);
            indexer = indexer.WalkTo(Direction.NW);
            OccupiedCellsLocations.Add(indexer);
            indexer = indexer.WalkTo(Direction.NW);
            OccupiedCellsLocations.Add(indexer);

            // now set the Mapcells at locations found to be not walkable..
            foreach (var location in OccupiedCellsLocations)
	        {
		        MapRef.MapCellAt(location).Walkable = false;
	        }

            //setting depths of irregular occupied cells..
            float irregularDepth1;
            float irregularDepth2;
            indexer = Location.WalkTo(Direction.SW).WalkTo(Direction.NW).WalkTo(Direction.NW);
            irregularDepth1 = MapRef.MapCellAt(indexer).DrawDepth;
            indexer = indexer.WalkTo(Direction.NW);
            irregularDepth2 = MapRef.MapCellAt(indexer).DrawDepth;

            int segIndexer = (ImgSegmentsDepthsOrdered.Count / 2) - 3; // indexes segment no. 3 counting from middle segment leftwards
            SegDepth newSegment = new SegDepth(ImgSegmentsDepthsOrdered[segIndexer].SegRect, irregularDepth1);
            ImgSegmentsDepthsOrdered[segIndexer] = newSegment;
            segIndexer--;
            SegDepth newSegment2 = new SegDepth(ImgSegmentsDepthsOrdered[segIndexer].SegRect, irregularDepth1);
            ImgSegmentsDepthsOrdered[segIndexer] = newSegment2;
            segIndexer--;
            SegDepth newSegment3 = new SegDepth(ImgSegmentsDepthsOrdered[segIndexer].SegRect, irregularDepth2);
            ImgSegmentsDepthsOrdered[segIndexer] = newSegment3;
#endregion 

            #region Generating Custom Built Units Locations
           //remove last location
            BuiltUnitsLocations.RemoveAt(BuiltUnitsLocations.Count-1);
            //get the new last location
            var loc = BuiltUnitsLocations[BuiltUnitsLocations.Count-1];

            Vector2 cell = new Vector2(MapRef.WorldToMapCell(loc).X,
                    MapRef.WorldToMapCell(loc).Y);

            cell = cell.WalkTo(Direction.SW);
            BuiltUnitsLocations.Add(MapRef.MapCellAt(cell).CenterPosition);
            cell = cell.WalkTo(Direction.NW);
            BuiltUnitsLocations.Add(MapRef.MapCellAt(cell).CenterPosition);
            cell = cell.WalkTo(Direction.NW);
            BuiltUnitsLocations.Add(MapRef.MapCellAt(cell).CenterPosition);
            
            #endregion

            // making smoke on the roof
            #region smoke
            _smoke = new SpriteAnimation(GameGraphics.GetTexture("smoke").SourceTexture);
            _smoke.AddAnimation("normal", 0, 0, 30, 64, 16, 0.1f);
            _smoke.Position = Camera.WorldToScreen(new Vector2( this.BuildingOrigin.X+85, this.BuildingOrigin.Y-60));
            _smoke.CurrentAnimation = "normal";
            _smoke.DrawDepth = BuildingInfo.SmokeDrawDepth;
            _smoke.IsAnimating = true;
            #endregion

            selectionGroup = GroupMapper.CampGroup;

            SelectionLineTexture = GameGraphics.GetTexture("camp_selection_line");

            #region Setting Entity Properties
            MaxHealth = 800;
            MaxDefense = 300;
            MaxAttack = 0;

            Health = MaxHealth;
            Defense = MaxDefense;
            Attack = MaxAttack;
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _smoke.Draw(spriteBatch, 0, 0);
          
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _smoke.Update(gameTime);
        }

        public override void Select()
        {
            base.Select();
            GameWorldRef.DockManager.ShowCampDock(this, PlayerRef);
        }

        public override void DeSelect()
        {
            base.DeSelect();
            GameWorldRef.DockManager.HideCurrentDock();
        }

        public override string UnitDiscription
        {
            get { return "Camp (Trains Soldiers)"; }
        }

    }
}
