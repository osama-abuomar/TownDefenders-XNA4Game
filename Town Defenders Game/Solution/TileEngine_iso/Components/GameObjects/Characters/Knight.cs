using Engine.Animation;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components.GameObjects.Characters
{
    public class Knight : CreatureBase, IEntity
    {

        private const float WalkingSpeed = 0.8f;
        private const float AnimationSpeed = 0.1f;

        public Knight(GameWorld gameWorldRef, GamePlayer playerRef, Vector2 position)
            : base("green_knight_ss", 48, 48, 8, gameWorldRef, playerRef, position, AnimationSpeed)
        {
            CharSelect = GameGraphics.GetTexture("char_select");
            Speed = WalkingSpeed;
            SelectionGroup = GroupMapper.KnightGroup;
            Animation.DrawOffset = new Vector2(-24, -38);

            #region More Animation
            #endregion

            SelectBounds = new Rectangle((int)position.X - 16,
                (int)position.Y - 32, 48 - 16, 48 - 16);

            #region Setting Entity Properties
            MaxHealth = 185;
            MaxDefense = 65;
            MaxAttack = 33;

            Health = MaxHealth;
            Defense = MaxDefense;
            Attack = MaxAttack;
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Select()
        {
            base.Select();
            GameWorldRef.DockManager.ShowKnightDock(this, PlayerRef);
        }

        public override void DeSelect()
        {
            base.DeSelect();
            GameWorldRef.DockManager.HideCurrentDock();
        }



        public override string UnitDiscription
        {
            get { return "Swordsman (soldier)"; }
        }


    }
}
