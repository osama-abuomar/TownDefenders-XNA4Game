using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Engine.Utilities.Debug
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {
        readonly ContentManager _content;
        SpriteBatch _spriteBatch;
        SpriteFont _spriteFont;

        int _frameRate = 0;
        int _frameCounter = 0;
        TimeSpan _elapsedTime = TimeSpan.Zero;
        private const int Infinity = int.MaxValue;


        public FrameRateCounter(Game game)
            : base(game)
        {
            _content = game.Content;
            DrawOrder = Infinity;
        }


        protected override void LoadContent()
        {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _spriteFont = _content.Load<SpriteFont>(@"Fonts\sf1");
        }


        protected override void UnloadContent()
        {
                _content.Unload();
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            
            _frameCounter++;

            string fps = string.Format("{0}", _frameRate);

            _spriteBatch.Begin();
            
            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(33, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(32, 32), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
