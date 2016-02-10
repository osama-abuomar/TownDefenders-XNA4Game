using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Engine.Utilities.Controls;
using Engine.Utilities;




namespace XNA_Game.GameScreens
{
    public class TitleScreen : BaseGameState
    {

        LinkLabel _startLabel;
        private PictureBox _background;
        private PictureBox _seperator;

        public TitleScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }

        protected override void LoadContent()
        {
            GameRef.IsMouseVisible = true;

            base.LoadContent();

            var _backgroundImage = GameGraphics.GetTexture("title_screen_bg").SourceTexture;
            _background = new PictureBox(_backgroundImage, GameRef.ScreenRectangle);

            var _seperatorImage = GameGraphics.GetTexture("seperator").SourceTexture;
            _seperator = new PictureBox(_seperatorImage, new Rectangle(0, 0, _seperatorImage.Width, _seperatorImage.Height));

            _startLabel = new LinkLabel();
            _startLabel.Text = "Start Menu";
            _startLabel.TabStop = true;
            _startLabel.HasFocus = true;
            _startLabel.LeftIndexer = true;
            _startLabel.Hover += new EventHandler(_startLabel_Hover);
            _startLabel.Click += new EventHandler(startLabel_Selected);

            UpdateLayout();

            _startLabel.Selected += new EventHandler(startLabel_Selected);
            ControlManager.Add(_startLabel);


        }

        void _startLabel_Hover(object sender, EventArgs e)
        {
            ControlManager.LoseFocus();
            ((Control)sender).HasFocus = true;
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);

            _background.Draw(GameRef.SpriteBatch);
            ControlManager.Draw(GameRef.SpriteBatch);
            _seperator.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        private void startLabel_Selected(object sender, EventArgs e)
        {
            StateManager.PushState(GameRef.StartMenuScreen);
        }

        public override void UpdateLayout()
        {
            _background.DestinationRectangle = GameRef.ScreenRectangle;

            // label position..
            int startLableX = GameRef.ScreenRectangle.Width / 2 - (int)ControlManager.SpriteFont.MeasureString(_startLabel.Text).X / 2;
            int startLableY = (int)(4f / 5f * GameRef.ScreenRectangle.Height);
            _startLabel.SetPosition(new Vector2(startLableX, startLableY));

            int seperatorXPos = GameRef.ScreenRectangle.Width / 2 - _seperator.Image.Width / 2;
            int seperatorYPos = startLableY - 65;
            _seperator.SetPosition(new Vector2(seperatorXPos, seperatorYPos));

        }
    }
}
