using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Engine.Utilities.Controls;
using Engine.Utilities;
using Microsoft.Xna.Framework.Input;


namespace XNA_Game.GameScreens
{
    public class Dialog : BaseGameState
    {

        private PictureBox _background;

        public Dialog(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }

        protected override void LoadContent()
        {
            GameRef.IsMouseVisible = true;

            base.LoadContent();

            var backgroundImage = GameRef.Content.Load<Texture2D>(@"Backgrounds\dialog");
            _background = new PictureBox(backgroundImage, GameRef.ScreenRectangle);
            ControlManager.Add(_background);

            Label pause = new Label();
            pause.Text = "Game Paused";
            pause.SetPosition
                (
                new Vector2(
                    (GameRef.ScreenWidth-ControlManager.SpriteFont.MeasureString(pause.Text).X)/2f+16,
                350)
                );
            ControlManager.Add(pause);


        }

        public override void Update(GameTime gameTime)
        {
            if(InputManager.KeyPressed(Keys.P))
                StateManager.PopState();
            ControlManager.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);
            ControlManager.Draw(GameRef.SpriteBatch);
            
          
            GameRef.SpriteBatch.End();
        }


    }
}
