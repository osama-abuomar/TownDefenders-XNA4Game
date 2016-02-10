using System;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects;
using Engine.Components.GameObjects.Characters;
using Engine.Components.GameObjects.Structures.Buildings;
using Engine.Iso_Tile_Engine;
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.Dock
{
    public class StableDock:DisplayDock
    {
        private Stable Context;

        public StableDock(Vector2 position, ControlManager controlManager) 
            : base(position, controlManager)
        {
            Thumbnail = new ImageButton96(
                 GameGraphics.GetTexture("stable_96").SourceTexture, ThumbnailPosition);
            Controls.Add(Thumbnail);

            #region create buttons for the farmer
            ImageButton48 createHorsemanBtn = new ImageButton48(
               GameGraphics.GetTexture("horseman_create_button").SourceTexture,
               ButtonsPositions[0]);
            createHorsemanBtn.Click += new System.EventHandler(createHorsemanBtn_Click);
            Controls.Add(createHorsemanBtn);


            #endregion

            // add all controls
            AddControls();
        }

        void createHorsemanBtn_Click(object sender, System.EventArgs e)
        {
            Vector2 loc = Context.NextBuiltUnitLocation;
            Vector2 modifiedLoc = new Vector2(loc.X, loc.Y + 20);
            Horseman hm = new Horseman(Context.GameWorldRef, Context.PlayerRef, modifiedLoc );
            hm.CurrentAnimation = Context.NextIdleAnimation;
            Context.PlayerRef.People.Add(hm);
        }

        /// <summary>
        /// updates the data source and command operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        public virtual void UpdateContext(Stable context, GamePlayer player)
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
