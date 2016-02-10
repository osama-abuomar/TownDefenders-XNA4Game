using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameObjects;
using Engine.Components.GameObjects.Characters;
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.Dock
{
    public class SwordsmanDock:DisplayDock
    {

        private Swordsman Context;

        public SwordsmanDock(Vector2 position, ControlManager controlManager) 
            : base(position, controlManager)
        {
            Thumbnail = new ImageButton96(
                GameGraphics.GetTexture("swordsman_96").SourceTexture, ThumbnailPosition);
            
            Controls.Add(Thumbnail);

            #region create buttons for the farmer
            #endregion

            // add all controls
            AddControls();
        }

        
        /// <summary>
        /// updates the data source and command operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        public virtual void UpdateContext(Swordsman context, GamePlayer player)
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
