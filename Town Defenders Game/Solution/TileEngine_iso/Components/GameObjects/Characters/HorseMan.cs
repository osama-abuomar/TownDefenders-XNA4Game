using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Characters
{
    public class Horseman : CreatureBase, IEntity
    {

        private const float WalkingSpeed = 1.7f;
        private const float AnimationSpeed = 0.05f;

        public Horseman(GameWorld gameWorldRef, GamePlayer playerRef, Vector2 position)
            : base("horseman_ss", 86, 86, 12, gameWorldRef, playerRef, position, AnimationSpeed)
        {
            CharSelect = GameGraphics.GetTexture("horseman_select");
            Speed = WalkingSpeed;
            SelectionGroup = GroupMapper.HorsemanGroup;
            Animation.DrawOffset = new Vector2(-45, -60);



            SelectBounds = new Rectangle((int)position.X - -16,
                (int)position.Y - 40, 50, 50);

            #region Setting Entity Properties
            MaxHealth = 90;
            MaxDefense = 30;
            MaxAttack = 6;

            Health = MaxHealth;
            Defense = MaxDefense;
            Attack = MaxAttack;
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PreviousMoveDirection == Vector2.Zero &&
                MoveDirection != Vector2.Zero)
            {
                SoundManager.Instance().PlaySoundEffect("horse_running");
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Select()
        {
            base.Select();
            SoundManager.Instance().PlaySoundEffect
                ("horse" + (new Random().Next(1, 3)));
            GameWorldRef.DockManager.ShowHorsemanDock(this, PlayerRef);
        }

    
        public override void DeSelect()
        {
            base.DeSelect();
            GameWorldRef.DockManager.HideCurrentDock();
        }



        public override string UnitDiscription
        {
            get { return "Horseman (Scout)"; }
        }


    }
}
