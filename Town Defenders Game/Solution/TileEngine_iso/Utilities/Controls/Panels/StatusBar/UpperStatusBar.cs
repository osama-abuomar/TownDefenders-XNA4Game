using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Engine.Components.GameLogic;
using Engine.Components.GameLogic.Player;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls.Panels.StatusBar
{
    public class UpperStatusBar
    {

        private readonly ControlManager ControlManager;
        private readonly GamePlayer Context;

        private Label resources, score, elapsedTime;

        private Vector2 OriginPosition;
        private static readonly Color LabelColor = new Color(224,186,151);


        public UpperStatusBar(Vector2 Position,GamePlayer context, ControlManager controlManager)
        {
            this.ControlManager = controlManager;
            this.Context = context;
            this.OriginPosition = Position;

            #region setting labels
            resources = new Label();
            resources.SpriteFont = GameFonts.GetFont("f5");
            resources.Color = LabelColor;
            resources.SetPosition(GetAbsolutePosition(90,3));
            resources.Text = "Resources: "+Context.Resources;
            ControlManager.Add(resources);

            score = new Label();
            score.SpriteFont = GameFonts.GetFont("f5");
            score.Color = LabelColor;
            score.SetPosition(GetAbsolutePosition(90+160, 3));
            score.Text = "Score: " + Context.Score;
            ControlManager.Add(score);

            elapsedTime = new Label();
            elapsedTime.SpriteFont = GameFonts.GetFont("f5");
            elapsedTime.Color = LabelColor;
            elapsedTime.SetPosition(GetAbsolutePosition(90+160+160, 3));
            elapsedTime.Text = "Population: "+Context.Population;
            ControlManager.Add(elapsedTime);
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            //binding again
            resources.Text = "Resources: " + Context.Resources;
            score.Text = "Score: " + Context.Score;
            elapsedTime.Text = "Population: " + Context.Population;
        }

        protected Vector2 GetAbsolutePosition(int x, int y)
        {
            var relativePosition = new Vector2(x, y);
            return relativePosition + OriginPosition;
        }

    }
}
