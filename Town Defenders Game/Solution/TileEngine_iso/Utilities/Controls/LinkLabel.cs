
using System;
using Engine.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Engine.Utilities.Controls
{
    public class LinkLabel : Control
    {



        public Color SelectedColor { get; set; }
        private PictureBox _leftIndexerPic;
        private PictureBox _rightIndexerPic;

        public bool LeftIndexer;
        public bool RightIndexer;
        

        protected Rectangle MouseActiveArea
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)SpriteFont.MeasureString(Text).X,
                    (int)SpriteFont.MeasureString(Text).Y);
            }
        }

        public LinkLabel()
        {
            SelectedColor = SelectColor;
            TabStop = true;
            HasFocus = false;
            Position = Vector2.Zero;
            Color = NormalColor;
            LeftIndexer = true;
            RightIndexer = true;
           
        }

        public override void SetPosition(Vector2 position)
        {

            Position = position;

            var LIndexerTexture = GameGraphics.GetTexture("left_indexer").SourceTexture;
            var RIndexerTexture = GameGraphics.GetTexture("right_indexer").SourceTexture;

            // setting left indexer
            var indexerXPosition = Position.X - LIndexerTexture.Width - 18; // some offset
            var indexerYPosition = Position.Y -3;
            var indexerPosition = new Vector2(indexerXPosition, indexerYPosition);
            _leftIndexerPic = new PictureBox(LIndexerTexture, new Rectangle(0, 0, LIndexerTexture.Width, LIndexerTexture.Height));
            _leftIndexerPic.SetPosition(indexerPosition);

            // setting right indexer
            indexerXPosition = Position.X + SpriteFont.MeasureString(Text).X + 18;
            indexerPosition = new Vector2(indexerXPosition, indexerYPosition);
            _rightIndexerPic = new PictureBox(RIndexerTexture, new Rectangle(0, 0, RIndexerTexture.Width, RIndexerTexture.Height));
            _rightIndexerPic.SetPosition(indexerPosition);

            _leftIndexerPic.Fading = true;
            _rightIndexerPic.Fading = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (HasFocus)
            {
                // Do what a ControlManager does in a GameState .. (update the control PictureBox)
                _leftIndexerPic.Update(gameTime);
                _rightIndexerPic.Update(gameTime);
            }
            else
            {
                _leftIndexerPic.ResetFadingState();
                _rightIndexerPic.ResetFadingState();
            }
            if (InputManager.MouseHoverOverArea(MouseActiveArea))
            {
                OnHover();
            }
            if (InputManager.MouseEnteredArea(MouseActiveArea))
            {
                SoundManager.Instance().PlaySoundEffect("focus");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SpriteFont, Text, Position, HasFocus ? SelectedColor : Color);
            if (HasFocus && LeftIndexer)
            {
                _leftIndexerPic.Draw(spriteBatch);
                _rightIndexerPic.Draw(spriteBatch);
            }

        }

        /// <summary>
        /// called by ControlManager when HasFocus
        /// </summary>
        public override void HandleInput()
        {
            if (HasFocus)
            {
                if (InputManager.KeyReleased(Keys.Enter))
                {
                    SoundManager.Instance().PlaySoundEffect("select");
                    base.OnSelected(null);
                }

                if (InputManager.MouseClickedOverArea(MouseActiveArea))
                {
                    SoundManager.Instance().PlaySoundEffect("select");
                    OnClick();
                }

                if (InputManager.MouseExitedArea(MouseActiveArea))
                {
                    HasFocus = false;
                }
            }

           
        }
    }
}

