using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Components.GameLogic.Movement;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Buildings;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions
{
    class CarryToHouseAction : IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;
        private ResidentialHouse _storageHouse;


        public CarryToHouseAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
            foreach (var building in player.Buildings)
            {
                if (building is ResidentialHouse)
                {
                    _storageHouse = building as ResidentialHouse;
                    break;
                }
            }
        }

        public bool Holds
        {
            get { return _farmer.CarryToHouse; }
        }

        public void Apply()
        {
            
            Vector2 v2HouseCellLoc = new Vector2(_storageHouse.Location.X,
                _storageHouse.Location.Y);

            UnitMovementManager.SetMovableToMovePath(
                _farmer,
                _farmer.CellLocation,
                v2HouseCellLoc.WalkTo(Direction.S),
                _player._gameWorldRef.Map
                );

            _farmer.UpdateMovingAnimation();

            _farmer.CarryToHouse = false;
            // needed we farmer reaches the building to know that he should go back to farm
            _farmer.CameFromAFarm = true;

            // needed by the retrun from building to farm action
            _farmer.CellPositionOnWorkingFarm = _farmer.CellLocation;

            Console.WriteLine("carrying to house");
        }
    }
}
