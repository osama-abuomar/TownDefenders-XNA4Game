using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Structures.Farm
{
    public class Farm : FarmBase
    {
        public int Food;
        public int MaxFood;
        public string FoodDescription 
        {
            get { return "Food: " + Food;}
        }

        public Farm(string identifier, GameWorld gameWorldRef, GamePlayer playerRef, Point location)
            : base(identifier, gameWorldRef, playerRef, location)
        {
            SelectionLineTexture = GameGraphics.GetTexture("farm_selection_line");

            SelectionGroup = GroupMapper.FarmGroup;

            MaxFood = 300;
            Food = MaxFood;
        }

        public override void Select()
        {
            base.Select();

        }

        public override void DeSelect()
        {
            base.DeSelect();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override string UnitDiscription
        {
            get { return "Farm (Food Source)"; }
        }
    }


}
