using System.Collections.Generic;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Buildings;
using Engine.Components.GameObjects.Structures.Farm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameLogic.Player
{
    public class GamePlayer
    {
        public List<CreatureBase> People;
        public List<BuildingBase> Buildings;

        public int Resources = 300;
        public int Score = 0;
        public string Name;

        public GameWorld _gameWorldRef;
        public List<Farm> Farms; 
       

        public GamePlayer(GameWorld gameWorldRef)
        {
            _gameWorldRef = gameWorldRef;


            People = new List<CreatureBase>();
            Buildings = new List<BuildingBase>();

            BuildingBase building3 = new ResidentialHouse(gameWorldRef, this, new Point(15, 20));
            BuildingBase building6 = new ResidentialHouse(gameWorldRef, this, new Point(6, 20));
            BuildingBase building4 = new TrainingCamp(gameWorldRef, this, new Point(10, 16));
            BuildingBase building1 = new TrainingCamp(gameWorldRef, this, new Point(30, 30));
            BuildingBase building5 = new Stable(gameWorldRef, this, new Point(14, 35));

            Buildings.Add(building6); Buildings.Add(building4);
            Buildings.Add(building5); Buildings.Add(building1);
            Buildings.Add(building3);

            Farms = new List<Farm>();
            Farms.Add(new Farm("farm1", gameWorldRef, this, new Point(4, 28)));
            Farms.Add(new Farm("farm1", gameWorldRef, this, new Point(6, 32)));
            Farms.Add(new Farm("farm1", gameWorldRef, this, new Point(10, 32)));
            Farms.Add(new Farm("farm1", gameWorldRef, this, new Point(12, 28)));

            //TODO: create player presets instead of following
            CreatureBase human1 =
                new Swordsman(gameWorldRef, this, new Vector2(300, 400));
            CreatureBase human2 =
                new Horseman(gameWorldRef, this, new Vector2(350, 400));
            CreatureBase human3 =
                  new Farmer(gameWorldRef, this, new Vector2(300, 460));
            CreatureBase human4 =
                  new Knight(gameWorldRef, this, new Vector2(350, 460));


           

            

            People.Add(human1); People.Add(human4); People.Add(human3); People.Add(human2);
            

        }

        public int Population
        {
            get { return People.Count; }

        }

        public void Update(GameTime gameTime)
        {

            foreach (Farm farm in Farms)
            {
                farm.Update(gameTime);
            }
            foreach (var item in Buildings)
            {
                item.Update(gameTime);
            }
            foreach (var item in People)
            {
                item.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in People)
            {
                item.Draw(spriteBatch);
            }

            foreach (var item in Buildings)
            {
                item.Draw(spriteBatch);
            }
            foreach (Farm farm in Farms)
            {
                 farm.Draw(spriteBatch);
            }

        }

    }
}
