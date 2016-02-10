using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions
{
    class SeedAction:IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;
        private long _timer = 0;

        public SeedAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
        }

        public bool Holds 
        {
            get
            {
                return _farmer.GoingToFarm &&
                       !_farmer.IsMoving &&
                       IsFarmerOnFarm();
            }
        }

        /// <summary>
        /// if true, it sets the working farm of the farmer to be
        /// the farm he is currently on top of.
        /// </summary>
        /// <returns></returns>
        private bool IsFarmerOnFarm()
        {
            var farmFound = false;
            var pPos = new Point((int) _farmer.Position.X, (int) _farmer.Position.Y);
            foreach (var farm in _player.Farms)
            {
                if (farm.SelectBounds.Contains(pPos))
                {
                    _farmer.WorkingFarm = farm;
                    farmFound = true;
                    break;
                }
            }
            return farmFound;
        }

        public void Apply()
        {
            _farmer.GoingToFarm = false;
            _farmer.Seeding = true;
            _farmer.CurrentAnimation = "seeding";
            _farmer.Timer = 0L;
            _farmer.TimerOn = true;
            Console.WriteLine("seeding");
        }
    }
}
