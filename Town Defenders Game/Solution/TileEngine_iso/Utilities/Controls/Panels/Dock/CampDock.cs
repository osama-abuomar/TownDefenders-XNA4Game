using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Buildings;
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.Dock
{
    public class CampDock:DisplayDock
    {
        private TrainingCamp Context;

        public CampDock(Vector2 position, ControlManager controlManager) 
            : base(position, controlManager)
        {
            Thumbnail = new ImageButton96(
                 GameGraphics.GetTexture("trainingcamp_96").SourceTexture, ThumbnailPosition);
            
            Controls.Add(Thumbnail);

            #region create buttons for the farmer
            ImageButton48 createSwordsmanBtn = new ImageButton48(
              GameGraphics.GetTexture("swordsman_create_button").SourceTexture,
              ButtonsPositions[0]);
            createSwordsmanBtn.Click += new System.EventHandler(createSwordsmanBtn_Click);
            Controls.Add(createSwordsmanBtn);

            ImageButton48 createKnightBtn = new ImageButton48(
              GameGraphics.GetTexture("knight_create_button").SourceTexture,
              ButtonsPositions[1]);
            createKnightBtn.Click += new System.EventHandler(createKnightBtn_Click);
            Controls.Add(createKnightBtn);
            #endregion

            // add all controls
            AddControls();
        }

        void createKnightBtn_Click(object sender, System.EventArgs e)
        {
            Vector2 loc = Context.NextBuiltUnitLocation;
            Knight knight = new Knight(Context.GameWorldRef, Context.PlayerRef, loc);
            knight.CurrentAnimation = Context.NextIdleAnimation;
            Context.PlayerRef.People.Add(knight);
        }

        void createSwordsmanBtn_Click(object sender, System.EventArgs e)
        {
            Vector2 loc = Context.NextBuiltUnitLocation;
            Swordsman swordsman = new Swordsman(Context.GameWorldRef, Context.PlayerRef, loc);
            swordsman.CurrentAnimation = Context.NextIdleAnimation;
            Context.PlayerRef.People.Add(swordsman);
        }

        /// <summary>
        /// updates the data source and command operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        public virtual void UpdateContext(TrainingCamp context, GamePlayer player)
        {
            Context = context;
            Player = player;
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            var entity = Context as IEntity;
            DisplayText.Text = Player.Name + "\n" +
                               entity.HealthDiscription + "\n" +
                               entity.AttackDiscription + "\n" +
                               entity.DefenseDiscription;

        }
    }
}
