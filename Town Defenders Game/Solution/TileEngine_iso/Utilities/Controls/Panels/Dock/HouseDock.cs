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
    public class HouseDock:DisplayDock
    {
        private ResidentialHouse Context;

        public HouseDock(Vector2 position, ControlManager controlManager) 
            : base(position, controlManager)
        {
            Thumbnail = new ImageButton96(
                 GameGraphics.GetTexture("house_96").SourceTexture, ThumbnailPosition);
            Controls.Add(Thumbnail);

            #region create buttons for the farmer
            ImageButton48 createFarmerBtn = new ImageButton48(
                GameGraphics.GetTexture("farmer_create_button").SourceTexture,
                ButtonsPositions[0]);
            createFarmerBtn.Click += new System.EventHandler(createFarmerBtn_Click);
            Controls.Add(createFarmerBtn);

            
            #endregion

            //last statement - add all controls to the control manager
            AddControls();
        }

        void createFarmerBtn_Click(object sender, System.EventArgs e)
        {
            Vector2 loc = Context.NextBuiltUnitLocation;
            Farmer farmer = new Farmer(Context.GameWorldRef, Context.PlayerRef, loc);
            farmer.CurrentAnimation = Context.NextIdleAnimation;
            Context.PlayerRef.People.Add(farmer);
        }

        /// <summary>
        /// updates the data source and command operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        public virtual void UpdateContext(ResidentialHouse  context, GamePlayer player)
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
