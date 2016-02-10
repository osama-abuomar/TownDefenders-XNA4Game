using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Characters;
using Microsoft.Xna.Framework;

namespace Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions
{
    class SpreadAction:IAction
    {
        private readonly Farmer _farmer;
        private readonly GamePlayer _player;
  

        public SpreadAction(Farmer farmer, GamePlayer player)
        {
            _farmer = farmer;
            _player = player;
        }

        public bool Holds 
        {
            get
            {
                return _farmer.Seeding &&
                       _farmer.Timer > _farmer.MaxSeedTimeMs;
            }
        }

        public void Apply()
        {
            _farmer.CurrentAnimation = "spreading";
            _farmer.Seeding = false;
            _farmer.Spreading = true;
            _farmer.Timer = 0L;
            _farmer.TimerOn = false;

            _farmer.GatheringTimer = 0L;
            _farmer.GatheringTimerOn = true;
            
            Console.WriteLine("spreading");
        }
    }
}
