
using Engine.Assets;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = Engine.Assets.Texture;

namespace Engine.Components.GameObjects.Views
{
    public enum StatusLineType
    {
        Green, Yellow
    }

    public class StatusLine
    {
        private Texture _belowLine, _topLine;
        private int _currentValue ;
        private int _maxValue ;
        private Vector2 _position;// screen position
        private float _ratio;
       
        private Rectangle _topSourceRect;
        private const float TopDepth = 0.1f, BelowDepth=0.1f+0.0001f;

        public StatusLine(StatusLineType type)
        {
            _belowLine = GameGraphics.GetTexture("red_bar");
            if (type == StatusLineType.Green)
                _topLine = GameGraphics.GetTexture("green_bar");
            else _topLine = GameGraphics.GetTexture("yellow_bar");
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw top
            spriteBatch.Draw(
                   _topLine.SourceTexture,
                    _position,
                    _topSourceRect,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    TopDepth
                   );

            //draw below
            spriteBatch.Draw(
                   _belowLine.SourceTexture,
                    _position,
                    _belowLine.SourceRectangle,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    BelowDepth
                   ); 

     
        }

        public void UpdateView(int value, int maxValue, Vector2 newPosition)
        {
            _position = Camera.WorldToScreen(newPosition); 
            
            _currentValue = value;
            _maxValue = maxValue;
            _ratio = (float)_currentValue / (float)_maxValue;

            _topSourceRect = new Rectangle(
                0, 0,
                (int)(_ratio * _topLine.SourceRectangle.Width),
                _topLine.SourceRectangle.Height);

           
        }
    }
}
