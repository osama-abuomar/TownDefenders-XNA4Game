using System.Collections.Generic;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.Dock
{
    public abstract class DisplayDock
    {
        protected ControlManager ControlManager;
        protected List<Control> Controls;
        protected Vector2 OriginPosition;
        protected ImageButton96 Thumbnail;
        protected Label DisplayText;
        protected GamePlayer Player;

        protected Vector2 ThumbnailPosition;
        protected Vector2 DisplayTextPosition;
        protected List<Vector2> ButtonsPositions;
        private const int MaxButtonCount = 5;

        /// <summary>
        /// initialize and all all components to the control manager
        /// </summary>
        /// <param name="position"></param>
        /// <param name="controlManager"></param>
        protected DisplayDock(Vector2 position, ControlManager controlManager)
        {
            OriginPosition = position;
            
            ButtonsPositions = new List<Vector2>();
            for (int i = 0; i < MaxButtonCount; i++)
            {
                ButtonsPositions.Add( GetAbsolutePosition(
                    10 + 96 + 10 + 200+i*(48+10),
                    10+5));
            }

            ThumbnailPosition = GetAbsolutePosition(10, 10);
            DisplayTextPosition = GetAbsolutePosition(10 + 96 + 10, 20);

            ControlManager = controlManager;
            Controls = new List<Control>();
            
            DisplayText = new Label();
            DisplayText.SetPosition(DisplayTextPosition);
            DisplayText.SpriteFont = GameFonts.GetFont("f4");
            DisplayText.Color = Control.NormalColor;
            Controls.Add(DisplayText);
        }

        protected Vector2 GetAbsolutePosition(int x, int y)
        {
            var relativePosition = new Vector2(x, y);
            return relativePosition + OriginPosition;
        }

        /// <summary>
        /// adds all controls to the control manager
        /// </summary>
        protected void AddControls()
        {
            foreach (var control in Controls)
            {
                ControlManager.Add(control);
            }
        }

     

        /// <summary>
        /// show everything
        /// </summary>
        public void Show()
        {
            foreach (var control in Controls)
            {
                control.Enabled = true;
                control.Visible = true;
            }
            Thumbnail.Enabled = false;
        }

        /// <summary>
        /// hide everything
        /// </summary>
        public void Hide()
        {
            foreach (var control in Controls)
            {
                control.Enabled = false;
                control.Visible = false;
            }
        }





    }
}
