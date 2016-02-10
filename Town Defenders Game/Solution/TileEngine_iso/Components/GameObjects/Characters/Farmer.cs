using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Actions;
using Engine.Components.GameLogic.Actions.ActionTypes.WorkerActions;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects.Structures.Farm;
using Engine.Components.GameObjects.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Characters
{
    public class Farmer : CreatureBase, IEntity
    {

        private const float WalkingSpeed = 0.8f;
        private const float AnimationSpeed = 0.1f;

        #region Farmer State Properties


        public bool GoingToFarm = false;
        public Farm WorkingFarm;
        public long MaxSeedTimeMs = 6000;
        public bool Seeding = false;
        public bool Spreading = false;
        public long MaxGatheringTimeMs = 5000;
        public long GatheringTimer = 0;
        public bool GatheringTimerOn = false;
        public bool CarryToHouse = false;
        public Vector2 CellPositionOnWorkingFarm;
        public bool CameFromAFarm = false;
        public int MaxResourceCapacity = 30;
        public int ResourceCapacity = 0;
        public int ResourceIncrement = 5;
        private StatusLine _capacityStatusLine;
        private Vector2 _capacityStatusLineOffset;

        #endregion

        public Farmer(GameWorld gameWorldRef, GamePlayer playerRef, Vector2 position)
            : base("farmer_ss", 48, 48, 8, gameWorldRef, playerRef, position, AnimationSpeed)
        {
            CharSelect = GameGraphics.GetTexture("char_select");
            Speed = WalkingSpeed;
            SelectionGroup = GroupMapper.FarmerGroup;
            Animation.DrawOffset = new Vector2(-24, -38);

            Animation.AddAnimation("seeding", 0, 8 * 48, 48, 48, 72, 0.1f);
            Animation.AddAnimation("spreading", 0, 9 * 48, 48, 48, 64, 0.1f);

            SelectBounds = new Rectangle((int)position.X - -16,
                (int)position.Y - 40, 50, 50);

            _capacityStatusLine = new StatusLine(StatusLineType.Yellow);
            _capacityStatusLineOffset = new Vector2(-13, -45);


            #region Setting Entity Properties
            MaxHealth = 50;
            MaxDefense = 5;
            MaxAttack = 3;

            Health = MaxHealth;
            Defense = MaxDefense;
            Attack = MaxAttack;

            #endregion

            #region Adding Possible Actions

            IAction action = new GoToFarmAction(this, playerRef);
            Actions.Add(action);

            IAction action2 = new SeedAction(this, playerRef);
            Actions.Add(action2);

            IAction action3 = new SpreadAction(this, playerRef);
            Actions.Add(action3);

            IAction action4 = new GatherAction(this, playerRef);
            Actions.Add(action4);

            IAction action5 = new CarryToHouseAction(this, playerRef);
            Actions.Add(action5);

            IAction action6 = new ReturnFromBuildingToFarmAction(this, playerRef);
            Actions.Add(action6);

            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // resource indecator update
            _capacityStatusLine.UpdateView(ResourceCapacity, MaxResourceCapacity,
                Position + _capacityStatusLineOffset);

            // update gathering timer if on
            if (GatheringTimerOn)
            {
                GatheringTimer += gameTime.ElapsedGameTime.Milliseconds;
            } 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsSelected)
                _capacityStatusLine.Draw(spriteBatch);
        }

        public override void Select()
        {
            base.Select();
            GameWorldRef.DockManager.ShowFarmerDock(this, PlayerRef);
        }


        public override void DeSelect()
        {
            base.DeSelect();
            GameWorldRef.DockManager.HideCurrentDock();
        }

        public override string UnitDiscription
        {
            get { return "Farmer (Worker)"; }
        }


    }
}
