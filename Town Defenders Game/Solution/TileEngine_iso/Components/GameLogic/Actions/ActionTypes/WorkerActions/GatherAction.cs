using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;

namespace Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions
{
    class GatherAction : IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;


        public GatherAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
        }

        public bool Holds
        {
            get
            {
                return _farmer.Spreading &&
                       _farmer.GatheringTimer > _farmer.MaxGatheringTimeMs;
            }
        }

        public void Apply()
        {
            _farmer.GatheringTimer = 0L;
            // keep gathering timer on

            _farmer.ResourceCapacity += _farmer.ResourceIncrement;
            Console.WriteLine("Gathered 5");

            // stop if capacity exeeded..
            if (_farmer.ResourceCapacity == _farmer.MaxResourceCapacity)
            {
                _farmer.GatheringTimer = 0L;
                _farmer.GatheringTimerOn = false;
                _farmer.Spreading = false;
                _farmer.CurrentAnimation = "Idle_NE";

                // needed by CarryToHouse State
                _farmer.CarryToHouse = true;
                
            }
        }
    }
}
