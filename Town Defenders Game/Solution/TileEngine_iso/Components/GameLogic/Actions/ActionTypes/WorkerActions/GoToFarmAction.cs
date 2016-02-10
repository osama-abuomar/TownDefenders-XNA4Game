using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Farm;
using Engine.Utilities;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions
{
    class GoToFarmAction : IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;

        public GoToFarmAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
        }

        public bool Holds
        {
            get
            {
                return (_farmer.IsSelected &&
                        isRCOnAFarm());
            }
        }

        public void Apply()
        {
            _farmer.GoingToFarm = true;
            Console.WriteLine("going to farm");
        }

        /// <summary>
        /// is right click on a farm (of the same player)
        /// </summary>
        /// <returns></returns>
        private bool isRCOnAFarm()
        {
            var RCHappened = InputManager.MouseRightButtonClicked();
            if (!RCHappened) return false;

            // mouse right click surely happened here..
            // chech if it is on any farm (the farmer is already going to the farm
            // by the movement manager)
            var v2mouseWPos = InputManager.GetMouseWorldLocation();
            var pMouseWPos = new Point((int)v2mouseWPos.X, (int)v2mouseWPos.Y);
            var playerFarms = _player.Farms;

            if (playerFarms.Any(farm => farm.SelectBounds.Contains(pMouseWPos)))
            {
                return true;
            }
            return false;
        }
    }
}
