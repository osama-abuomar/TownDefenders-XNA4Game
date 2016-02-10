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
    class ReturnFromBuildingToFarmAction : IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;
       

        public ReturnFromBuildingToFarmAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
           
        }

        public bool Holds
        {
            get
            {
                return _farmer.CameFromAFarm &&
                    !_farmer.IsMoving;
            }
        }

        public void Apply()
        {

            UnitMovementManager.SetMovableToMovePath(
                _farmer,
                _farmer.CellLocation,
                _farmer.CellPositionOnWorkingFarm,
                _player._gameWorldRef.Map
                );

            _farmer.UpdateMovingAnimation();

            _farmer.CameFromAFarm = false;

            // drop the resources here
            _player.Resources += _farmer.ResourceCapacity;
            _farmer.ResourceCapacity = 0;
            // just as done in the Go to farm Action (loop)
            _farmer.GoingToFarm = true;

            Console.WriteLine("returning to farm");
        }
    }
}
