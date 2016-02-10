
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls
{
    public class PictureBox : Control
    {
        public Texture2D Image { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public bool Fading;
        private float DrawDepth;

        public PictureBox(Texture2D image, Rectangle destination)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            Color = Color.White;
            Fading = false;
            DrawDepth = ControlInfo.ImageDrawDepth;
        }
        public PictureBox(Texture2D image, Rectangle destination, Rectangle source)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = source;
            Color = Color.White;
            Fading = false;
            DrawDepth = ControlInfo.ImageDrawDepth;
        }
        public PictureBox(Texture2D image, Rectangle destination, float drawDepth)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            Color = Color.White;
            Fading = false;
            DrawDepth = drawDepth;
        }
      

      

        public override void Update(GameTime gameTime)
        {
            if (Fading)
            {
                UpdateFadingBehaviour();
            }
        
        }

        #region Fading Behaviour
        private float _colorValueAsFloat = 255.0f;
        private const float MaxColorValue = 255.0f;
        private const float MinColorValue = 100f;
        private float _multiplyingFactor = 0.95f;
        private Color _tintColor;
        

        private void UpdateFadingBehaviour()
        {
            _colorValueAsFloat = (_colorValueAsFloat * _multiplyingFactor);
            var newColor = (int)_colorValueAsFloat;
            _tintColor = new Color(newColor, newColor, newColor, newColor);

            if (_colorValueAsFloat < MinColorValue)
            {
                _colorValueAsFloat = MinColorValue;
                _multiplyingFactor = 1.0f / _multiplyingFactor;
            }
            if (_colorValueAsFloat > MaxColorValue)
            {
                _colorValueAsFloat = MaxColorValue;
                _multiplyingFactor = 1.0f / _multiplyingFactor;
            }
        }
        #endregion

        public void ResetFadingState()
        {
            _multiplyingFactor = 0.95f;
            _colorValueAsFloat = 255.0f;
            _colorValueAsFloat = (_colorValueAsFloat * _multiplyingFactor);
            var newColor = (int)_colorValueAsFloat;
            _tintColor = new Color(newColor, newColor, newColor, newColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(
                  Image,
                   new Vector2(DestinationRectangle.X, DestinationRectangle.Y), 
                   SourceRectangle,
                   Fading ? _tintColor : Color,
                   0.0f,
                   Vector2.Zero,
                   1.0f,
                   SpriteEffects.None,
                   DrawDepth
                  );
        }

        public override void HandleInput()
        {
        }

        public override void SetPosition(Vector2 newPosition)
        {
            DestinationRectangle = new Rectangle(
            (int)newPosition.X,
            (int)newPosition.Y,
            SourceRectangle.Width,
            SourceRectangle.Height);
        }

      
    }
}
