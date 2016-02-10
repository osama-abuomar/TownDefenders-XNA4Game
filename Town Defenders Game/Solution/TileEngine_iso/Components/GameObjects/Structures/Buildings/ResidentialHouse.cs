using Engine.Animation;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Structures.Buildings
{
    public class ResidentialHouse : BuildingBase
    {
        private SpriteAnimation smoke;

      

        public ResidentialHouse(GameWorld gameWorldRef, GamePlayer playerRef, Point Location)
            : base("building1", gameWorldRef, playerRef, Location)
        {
            // filling the occupied_cells_locations List 
            // (the purpose is to make these cells not walkable)
            #region Occupied Cells Work
            int cells_east = 3, cells_west = 5;

            Point west_indexer = Location;
            Point east_indexer = Location;

            for (int i = 0; i < cells_west; i++)
            {
                for (int j = 0; j < cells_east; j++)
                {
                    OccupiedCellsLocations.Add(east_indexer);
                    east_indexer = east_indexer.WalkTo(Direction.NE);
                }
                west_indexer = west_indexer.WalkTo(Direction.NW);
                east_indexer = west_indexer;
            }

            // now set the Mapcells at locations found to be not walkable..
            foreach (var location in OccupiedCellsLocations)
	        {
		        MapRef.MapCellAt(location).Walkable = false;
            }
            #endregion

            // making smoke on the roof
            #region smoke
            smoke = new SpriteAnimation(GameGraphics.GetTexture("smoke").SourceTexture);
            smoke.AddAnimation("normal", 0, 0, 30, 64, 16, 0.15f);
            smoke.Position = Camera.WorldToScreen(new Vector2(this.BuildingOrigin.X + 65, this.BuildingOrigin.Y - 64 + 35));
            smoke.CurrentAnimation = "normal";
            smoke.DrawDepth = BuildingInfo.SmokeDrawDepth;
            smoke.IsAnimating = true;
            #endregion

            selectionGroup = GroupMapper.HouseGroup;

            SelectionLineTexture = GameGraphics.GetTexture("residence_selection_line");

            #region Setting Entity Properties
            MaxHealth = 500;
            MaxDefense = 100;
            MaxAttack = 0;

            Health = MaxHealth;
            Defense = MaxDefense;
            Attack = MaxAttack;
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            smoke.Draw(spriteBatch, 0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            smoke.Update(gameTime);
            UpdateSelectionHandling();
        }

        private void UpdateSelectionHandling()
        {
            GameWorldRef.SelectionManager.ManageSelection(this);
        }

        public override void Select()
        {
            base.Select();
            GameWorldRef.DockManager.ShowHouseDock(this, PlayerRef);
        }

        public override void DeSelect()
        {
            base.DeSelect();
            GameWorldRef.DockManager.HideCurrentDock();
        }

        public override string UnitDiscription
        {
            get { return "Residence Building"; }
        }
    }
}
